using Domain.Api;
using Domain.Identity.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Identity.Business
{
    public class TokenService : ITokenService
    {
        private AppSettings _appSettings;
        public TokenService(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }
        public string GenerateAccessToken(User user)
        {
           // Register claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _appSettings.AppUrl),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId)
            };

            // Key credentials
            var secretBytes = Encoding.ASCII.GetBytes(_appSettings.SaltFixed);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);

            // Create Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(
                _appSettings.AppUrl,
                _appSettings.AppUrl,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_appSettings.AccessTokenExpiration),
                signingCredentials: signingCredentials);
            
            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
                randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        public ClaimsPrincipal GetPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.InboundClaimTypeMap.Clear();

            var key = Encoding.ASCII.GetBytes(_appSettings.SaltFixed);
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            }, out SecurityToken securityToken);

            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException();
            return principal;
        }
        public ClaimsPrincipal Validate(string token, string audience)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.InboundClaimTypeMap.Clear();

                var key = Encoding.ASCII.GetBytes(_appSettings.SaltFixed);
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _appSettings.AppUrl,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException();
                return principal;
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ClaimsPrincipal Validate(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.InboundClaimTypeMap.Clear();

                var key = Encoding.ASCII.GetBytes(_appSettings.SaltFixed);
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _appSettings.AppUrl,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException();
                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool stat)
        {
            if (stat)
            {
                this._appSettings = null;
            }
        }
        //public async Task<EB_MasterUser> GetUserClaim(EB_MasterUser data)
        //{
        //    _uow.OpenConnection(ConnectionString.BMIIdentity);

        //    using (_uow)
        //    {
        //        using (var repo = new MasterUserRepository(_uow))
        //        {
        //            try
        //            {
        //                var response = await repo.ReadSingle(data);

        //                if (response == null)
        //                    throw new Exception(ApiMessage.Get(ApiMessageType.DataNotFound, "Account"));
        //                else
        //                    return response;
        //            }
        //            catch (Exception ex)
        //            {
        //                _uow.RollbackTransaction();
        //                throw ex;
        //            }
        //        }
        //    }
        //}
        //private async Task<string> GenerateCustomToken(EB_MasterUser Data, string IpAddress)
        //{
        //    var SaltFixed = _uow.AppSetting.Config.SaltFixed;
        //    var PassPhrase = _uow.AppSetting.Config.PassPhrase;
        //    var HashAlgorithm = _uow.AppSetting.Config.HashAlgorithm;
        //    var PasswordIterations = _uow.AppSetting.Config.PasswordIterations;
        //    var InitVector = _uow.AppSetting.Config.InitVector;
        //    var KeySize = _uow.AppSetting.Config.KeySize;

        //    AuditLogin auditData = new AuditLogin
        //    {
        //        UserID = Data.UserID,
        //        IPAddress = IpAddress
        //    };
        //    auditData = await AuditLogin(auditData);

        //    var userClaim = await GetUserClaim(Data);

        //    var obj = new UserClaim
        //    {
        //        UserID = auditData.UserID,
        //        UserEmail = userClaim.Email,
        //        UserName = userClaim.UserName,
        //        UserFullName = userClaim.FullName
        //    };

        //    var UserClaimResult = Helper.Encrypt(Helper.JSONSerialize(obj), PassPhrase, SaltFixed, HashAlgorithm, int.Parse(PasswordIterations), InitVector, int.Parse(KeySize));
        //    var UserKey = Helper.Encrypt(auditData.UserKey, PassPhrase, SaltFixed, HashAlgorithm, int.Parse(PasswordIterations), InitVector, int.Parse(KeySize));
        //    var Ip = Helper.Encrypt(IpAddress, PassPhrase, SaltFixed, HashAlgorithm, int.Parse(PasswordIterations), InitVector, int.Parse(KeySize));

        //    return UserClaimResult + "." + UserKey + "." + Ip;
        //}
    }
}
