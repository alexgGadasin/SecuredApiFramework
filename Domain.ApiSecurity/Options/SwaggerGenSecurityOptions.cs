using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ApiSecurity
{
    public class SwaggerGenSecurityOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly ISecurityConfiguration _securityOptions;
        public SwaggerGenSecurityOptions(ISecurityConfiguration securityOptions)
        {
            _securityOptions = securityOptions;
        }
        public void Configure(string name, SwaggerGenOptions options)
        {
            string IdentityAddress = _securityOptions.IdentityAddress;

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{IdentityAddress}/api/connect/authorize"),
                        TokenUrl = new Uri($"{IdentityAddress}/api/connect/token")
                    },
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{IdentityAddress}/api/connect/token")
                    }
                }
            });

            options.OperationFilter<SecurityOperationFilter>();
        }
        public void Configure(SwaggerGenOptions options) => Configure(Options.DefaultName, options);
    }
}
