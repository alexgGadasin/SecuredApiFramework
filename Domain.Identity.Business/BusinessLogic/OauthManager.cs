using Domain.Api;
using Domain.Identity.Data;
using Domain.Database;
using Domain.Identity.Oauth;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public class OauthManager : BaseBusinessLogic, IOauthManager
    {
        public OauthManager(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) : base(uow, appSettings, connectionStrings) { }
        public async Task<string> Authorize(AuthorizationRequest data)
        {
            if (data.response_type != "code")
                throw new OauthException(OauthErrorCode.unsupported_response_type);
            else
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
                    {
                        try
                        {
                            var currentData = await repo.ReadByLambda(u => u.Name == data.client_id && u.IsActive == true);

                            if (!currentData.Any())
                                throw new OauthException(OauthErrorCode.invalid_client);
                            else
                            {
                                var query = new QueryBuilder
                                {
                                    { "redirect_uri", data.redirect_uri },
                                    { "state", data.state }
                                };

                                return query.ToString();
                            }
                        }
                        catch (Exception)
                        {
                            _uow.RollbackTransaction();
                            throw;
                        }
                    }
                }
            }
        }
        public async Task<AuthorizationResponse> Authorize(AuthenticateRequest data, string state)
        {
            var validations = new List<ApiValidation>();

            _uow.OpenConnection(ConnectionString.DefaultConnection);

            using (_uow)
            {
                using (var repo = new UserRepository(_uow))
                {
                    try
                    {
                        var currentData = await repo.ReadByLambda(u => u.UserName == data.UserName && u.IsActive == true);

                        if (!currentData.Any())
                            throw new OauthException(OauthErrorCode.invalid_request, "User not found");
                        else
                        {
                            User currentUser = currentData.First();
                            data.Password = ApiHelper.HashWithSalt(data.Password, currentUser.SaltText, SHA384.Create());

                            if (data.Password != currentUser.Password)
                                throw new OauthException(OauthErrorCode.invalid_request, "Invalid password");
                            else
                            {
                                using (var repoCode = new AuthorizationCodeRepository(_uow))
                                {
                                    var AuthCode = await repoCode.GenerateCode(currentUser.UserId, _appSettings.CodeTokenExpiration);
                                    var response = new AuthorizationResponse
                                    {
                                        code = AuthCode.Code,
                                        state = state
                                    };

                                    return response;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        _uow.RollbackTransaction();
                        throw;
                    }
                }
            }
        }
        public async Task<AccessTokenResponse> Token(AccessTokenRequest data)
        {
            if (data.grant_type == "authorization_code")
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    ClientRepository clientRepo = new ClientRepository(_uow);
                    ClientIdentityClaimRepository clientIdentityClaimRepository = new ClientIdentityClaimRepository(_uow);
                    AuthorizationCodeRepository authCodeRepo = new AuthorizationCodeRepository(_uow);
                    UserRepository userRepo = new UserRepository(_uow);

                    try
                    {
                        var clients = await clientRepo.ReadByLambda(u => u.Name == data.client_id && u.IsActive == true);

                        if (!clients.Any())
                            throw new OauthException(OauthErrorCode.invalid_client);

                        var AuthCode = new AuthorizationCode() { Code = data.code };
                        var user_id = await authCodeRepo.FetchCode(AuthCode);
                        var users = await userRepo.ReadByLambda(u => u.UserId == user_id && u.IsActive == true);

                        if (!users.Any())
                            throw new OauthException(OauthErrorCode.invalid_request);

                        var currentClient = clients.First();
                        var currentUser = users.First();
                        var clientIdentityClaims = await clientIdentityClaimRepository.ReadByClient(currentClient.ClientId);

                        if (data.scopes.Any(s => !clientIdentityClaims.Any(cs => cs.ScopeName == s)))
                            throw new OauthException(OauthErrorCode.invalid_scope);

                        var access_token = await GetAccessToken(data, currentClient, currentUser);
                        var refresh_token = GetRefreshToken();

                        var response = new AccessTokenResponse
                        {
                            access_token = access_token,
                            token_type = "Bearer",
                            expires_in = _appSettings.AccessTokenExpiration.ToString(),
                            refresh_token = refresh_token,
                        };

                        return response;
                    }
                    catch (Exception)
                    {
                        _uow.RollbackTransaction();
                        throw;
                    }
                    finally
                    {
                        clientRepo.Dispose();
                        authCodeRepo.Dispose();
                    }
                }
            }
            else if (data.grant_type == "client_credentials")
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    ClientRepository clientRepo = new ClientRepository(_uow);
                    ClientIdentityClaimRepository clientIdentityClaimRepository = new ClientIdentityClaimRepository(_uow);
                    ClientSecretRepository clientSecretRepo = new ClientSecretRepository(_uow);

                    try
                    {
                        var clients = await clientRepo.ReadByLambda(u => u.Name == data.client_id && u.IsActive == true);

                        if (!clients.Any())
                            throw new OauthException(OauthErrorCode.invalid_client);

                        var currentClient = clients.First();
                        var clientSecrets = await clientSecretRepo.ReadByLambda(u => u.ClientId == currentClient.ClientId && u.Value == data.client_secret && u.IsActive == true);

                        if (!clientSecrets.Any())
                            throw new OauthException(OauthErrorCode.invalid_client);

                        var clientIdentityClaims = await clientIdentityClaimRepository.ReadByClient(currentClient.ClientId);

                        if (data.scopes.Any(s => !clientIdentityClaims.Any(cs => cs.ScopeName == s)))
                            throw new OauthException(OauthErrorCode.invalid_scope);

                        var access_token = await GetAccessToken(data, currentClient);

                        var response = new AccessTokenResponse
                        {
                            access_token = access_token,
                            token_type = "Bearer",
                            expires_in = _appSettings.AccessTokenExpiration.ToString()
                        };

                        return response;
                    }
                    catch (Exception)
                    {
                        _uow.RollbackTransaction();
                        throw;
                    }
                    finally
                    {
                        clientRepo.Dispose();
                        clientSecretRepo.Dispose();
                    }
                }
            }
            else if (data.grant_type == "refresh_token")
            {
                var access_token = GetAccessToken();
                var refresh_token = GetRefreshToken();

                var response = new AccessTokenResponse
                {
                    access_token = access_token,
                    token_type = "Bearer",
                    expires_in = _appSettings.AccessTokenExpiration.ToString(),
                    refresh_token = refresh_token
                };

                return response;
            }
            else
            {
                throw new OauthException(OauthErrorCode.unsupported_grant_type);
            }
        }
        private async Task<string> GetAccessToken(AccessTokenRequest data, Client client, User user)
        {
            ClientIdentityClaimRepository clientIdentityClaimRepository = new ClientIdentityClaimRepository(_uow);
            UserClaimRepository userClaimRepository = new UserClaimRepository(_uow);

            try
            {
                var clientIdentityClaims = await clientIdentityClaimRepository.ReadByClient(client.ClientId, data.scopes);
                var userClaims = await userClaimRepository.ReadByLambda(u => u.UserId == user.UserId && u.IsActive == true);

                // Register claims
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Iss, _appSettings.AppUrl),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                    new Claim("acc", "User")
                };

                foreach (string ResourceName in clientIdentityClaims.GroupBy(i => i.ResourceName).Select(i => i.Key))
                    claims.Add(new Claim(JwtRegisteredClaimNames.Aud, ResourceName));

                foreach (ClientIdentityClaim identityClaim in clientIdentityClaims)
                    claims.Add(new Claim("scope", identityClaim.ScopeName));

                foreach (UserClaim claim in userClaims)
                    claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));

                // Key credentials
                var issuer = _appSettings.AppUrl;
                //var audience = _appSettings.Config.AppUrl;
                var secret = _appSettings.SaltFixed;
                var expire_interval = _appSettings.AccessTokenExpiration;

                return OauthTokenManager.GenerateAccessToken(issuer, null, secret, claims, expire_interval);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clientIdentityClaimRepository.Dispose();
            }
        }
        private async Task<string> GetAccessToken(AccessTokenRequest data, Client client)
        {
            ClientIdentityClaimRepository clientIdentityClaimRepository = new ClientIdentityClaimRepository(_uow);

            try
            {
                var clientIdentityClaims = await clientIdentityClaimRepository.ReadByClient(client.ClientId, data.scopes);

                // Register claims
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Iss, _appSettings.AppUrl),
                    new Claim(JwtRegisteredClaimNames.Sub, client.Name),
                    new Claim("acc", "Client")
                };

                foreach (string ResourceName in clientIdentityClaims.GroupBy(i => i.ResourceName).Select(i => i.Key))
                    claims.Add(new Claim(JwtRegisteredClaimNames.Aud, ResourceName));

                foreach (ClientIdentityClaim identityClaim in clientIdentityClaims)
                    claims.Add(new Claim("scope", identityClaim.ScopeName));

                // Key credentials
                var issuer = _appSettings.AppUrl;
                //var audience = _appSettings.Config.AppUrl;
                var secret = _appSettings.SaltFixed;
                var expire_interval = _appSettings.AccessTokenExpiration;

                return OauthTokenManager.GenerateAccessToken(issuer, null, secret, claims, expire_interval);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clientIdentityClaimRepository.Dispose();
            }
        }
        private string GetAccessToken()
        {
            // Register claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, _appSettings.AppUrl),
                new Claim(JwtRegisteredClaimNames.Sub, "refresh"),
            };

            // Key credentials
            var issuer = _appSettings.AppUrl;
            var audience = _appSettings.AppUrl;
            var secret = _appSettings.SaltFixed;
            var expire_interval = _appSettings.AccessTokenExpiration;

            return OauthTokenManager.GenerateAccessToken(issuer, audience, secret, claims, expire_interval);
        }
        private string GetRefreshToken()
        {
            return OauthTokenManager.GenerateRefreshToken();
        }
        //public async Task<bool> ValidateCode(string code)
        //{
        //    var response = new ApiResponse();
        //    var validations = new List<ApiValidation>();

        //    _uow.OpenConnection(ConnectionString.BMIIdentity);

        //    using (_uow)
        //    {
        //        using (var repo = new AuthorizationCodeRepository(_uow))
        //        {
        //            try
        //            {
        //                return await repo.ValidateCode(new AuthorizationCode() { Code = code });
        //            }
        //            catch (Exception ex)
        //            {
        //                _uow.RollbackTransaction();
        //                throw ex;
        //            }
        //        }
        //    }
        //}
        //public async Task<bool> Token(string code, string client_id)
        //{
        //    var response = new ApiResponse();
        //    var validations = new List<ApiValidation>();

        //    _uow.OpenConnection(ConnectionString.BMIIdentity);

        //    using (_uow)
        //    {
        //        var authRepo = new AuthorizationCodeRepository(_uow);
        //        var userRepo = new MasterUserRepository(_uow);

        //        try
        //        {
        //            var authorizationCodes = await authRepo.ReadByLambda(c => c.Code == code);

        //            if (!authorizationCodes.Any())
        //                throw new Exception(ApiResult.Get(ApiMessageType.DataNotFound, "Code"));
        //            else
        //            {
        //                var UserId = await authRepo.FetchCode(authorizationCodes.First());
        //                var currentUser = await userRepo.ReadSingle(new MasterUser() { UserId = UserId });

        //                var AccessToken = _tokenService.GenerateAccessToken(currentUser);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            _uow.RollbackTransaction();
        //            return false;
        //        }
        //        finally
        //        {
        //            authRepo.Dispose();
        //            userRepo.Dispose();
        //        }
        //    }
        //}
    }
}
