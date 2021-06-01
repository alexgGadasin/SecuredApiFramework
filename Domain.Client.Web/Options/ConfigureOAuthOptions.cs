using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Domain.Client.Web
{
    public class ConfigureOAuthOptions : IConfigureOptions<OAuthOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;
        public ConfigureOAuthOptions(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }
        public void Configure(string name, OAuthOptions options)
        {
            options.ClientId = _appSettings.ClientId;
            options.ClientSecret = _appSettings.ClientSecret;
            options.CallbackPath = "/api/oauth/callback";
            options.AuthorizationEndpoint = $"{_appSettings.GatewayUrl}/api/connect/authorize";
            options.TokenEndpoint = $"{_appSettings.GatewayUrl}/api/connect/token";
            options.SaveTokens = true;

            options.Events = new OAuthEvents()
            {
                OnCreatingTicket = context =>
                {
                    var accessToken = context.AccessToken;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken token = tokenHandler.ReadJwtToken(accessToken);

                    foreach (var claim in token.Claims)
                    {
                        context.Identity.AddClaim(new Claim(claim.Type, claim.Value));
                    }

                    return Task.CompletedTask;
                }
            };
        }
        public void Configure(OAuthOptions options) => Configure(Options.DefaultName, options);
    }
}
