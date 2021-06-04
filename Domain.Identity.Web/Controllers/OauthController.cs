using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Domain.Api;
using Domain.Identity.Business;
using Domain.Identity.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Domain.Identity.Oauth;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Domain.Identity.Web
{
    [Route("api/connect")]
    [SkipApiExceptionFilter, OauthExceptionFilter]
    public class OauthController : Controller
    {
        private readonly IOauthManager _oauthCodeManager;
        private readonly IClient _client;
        private readonly IClientSecret _clientSecret;
        private readonly ITokenService _tokenService;
        private readonly AppSettings _appSettings;
        public OauthController(IServiceProvider serviceProvider, IOptions<AppSettings> appSettings)
        {
            this._oauthCodeManager = serviceProvider.GetRequiredService<IOauthManager>();
            this._client = serviceProvider.GetRequiredService<IClient>();
            this._clientSecret = serviceProvider.GetRequiredService<IClientSecret>();
            this._tokenService = serviceProvider.GetRequiredService<ITokenService>();
            this._appSettings = appSettings.Value;
        }
        [HttpGet, Route("authorize")]
        public async Task<IActionResult> Authorize(string response_type, string client_id, string redirect_uri, string scope, string state)
        {
            try
            {
                var requestData = new AuthorizationRequest()
                {
                    response_type = response_type,
                    client_id = client_id,
                    redirect_uri = redirect_uri,
                    scope = scope,
                    state = state
                };

                var response = await _oauthCodeManager.Authorize(requestData);
                return View(model: response);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost, Route("authorize")]
        public async Task<IActionResult> Authorize(string username, string password, string redirect_uri, string state)
        {
            try
            {
                var requestData = new AuthenticateRequest() { UserName = username, Password = password };
                var response = await _oauthCodeManager.Authorize(requestData, state);

                var query = new QueryBuilder
                    {
                        { "code", response.code },
                        { "state", response.state }
                    };

                return Redirect($"{redirect_uri}{query}");
            }
            catch (Exception ex)
            {
                if (ex is OauthException)
                {
                    OauthException oauthEx = ex as OauthException;
                    ViewBag.ErrorMessage = oauthEx.ErrorDescription.ToString();
                    var query = new QueryBuilder
                    {
                        { "redirect_uri", redirect_uri },
                        { "state", state }
                    };

                    return View(model: query.ToString());
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpPost, Route("token")]
        public async Task<IActionResult> Token(string grant_type, string code, string redirect_uri, string client_id, string client_secret, string scope, string refresh_token)
        {
            try
            {
                if (grant_type == "authorization_code")
                {
                    var requestData = new AccessTokenRequest()
                    {
                        grant_type = grant_type,
                        code = code,
                        redirect_uri = redirect_uri,
                        client_id = client_id
                    };

                    var data = await _oauthCodeManager.Token(requestData);
                    var response = new ApiResponse(HttpStatusCode.OK, data);

                    return this.SendResponse(response);
                }
                else if (grant_type == "client_credentials")
                {
                    var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                    var requestData = new AccessTokenRequest()
                    {
                        grant_type = grant_type,
                        client_id = client_id,
                        client_secret = client_secret,
                        scope = scope
                    };

                    if (requestData.client_id == null && authHeader != null)
                    {
                        try
                        {
                            var identity = ClientIdentifier.Get(authHeader);
                            requestData.client_id = identity.client_id;
                            requestData.client_secret = identity.client_secret;
                        }
                        catch (Exception) { }
                    }

                    var data = await _oauthCodeManager.Token(requestData);
                    var response = new ApiResponse(HttpStatusCode.OK, data);
                    return this.SendResponse(response);
                }
                else if (grant_type == "refresh_token")
                {
                    var requestData = new AccessTokenRequest()
                    {
                        grant_type = grant_type,
                        refresh_token = refresh_token
                    };

                    var data = await _oauthCodeManager.Token(requestData);
                    var response = new ApiResponse(HttpStatusCode.OK, data);
                    return this.SendResponse(response);
                }
                else
                {
                    throw new OauthException(OauthErrorCode.unsupported_grant_type);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet, Route("Validate")]
        public IActionResult Validate()
        {
            try
            {
                if (HttpContext.Request.Query.TryGetValue("access_token", out var accessToken))
                {
                    ClaimsPrincipal principal;

                    if (HttpContext.Request.Query.TryGetValue("resource", out var resource))
                    {
                        principal = _tokenService.Validate(accessToken, resource);
                    }
                    else
                    {
                        principal = _tokenService.Validate(accessToken);
                    }

                    List<ApiClaim> claims = new List<ApiClaim>();
                    foreach(Claim claim in principal.Claims)
                    {
                        claims.Add(new ApiClaim() { Type = claim.Type, Value = claim.Value });
                    }

                    return Ok(claims);
                }

                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
