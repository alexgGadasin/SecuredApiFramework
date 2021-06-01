
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Domain.Client.Web
{
    public static class ServiceRegistryExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            //services.ConfigureOptions<ConfigureOAuthOptions>();

            return services;
        }
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                // We check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                // When we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                // use this to check if we are allowed to do something.
                config.DefaultChallengeScheme = "OurServer";
            })
                .AddCookie("ClientCookie")
                .AddOAuth("OurServer", options =>
                {
                    options.ClientId = "sampleClient";
                    options.ClientSecret = "OaEa/9gu5NfQPH0pkehEm/fU/mBmyKCEaasiasNHDRU=";
                    options.CallbackPath = "/api/oauth/callback";
                    options.AuthorizationEndpoint = $"https://localhost:44310/api/connect/authorize";
                    options.TokenEndpoint = $"https://localhost:44310/api/connect/token";
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
                });

            return services;
        }
    }
}
