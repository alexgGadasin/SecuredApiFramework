using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.ApiSecurity
{
    public static class ServiceRegistryExtension
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.ConfigureOptions<SwaggerGenDocOptions>();

            return services;
        }
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddAuthentication("DefaultAuth")
                .AddScheme<AuthenticationSchemeOptions, SecurityAuthenticationHandler>("DefaultAuth", null);

            services.AddAuthorization(config =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .AddRequirements(new SecurityRequirement())
                    .Build();

                config.DefaultPolicy = defaultAuthPolicy;
            });

            services.AddScoped<IAuthorizationHandler, SecurityRequirementHandler>();
            services.AddHttpClient().AddHttpContextAccessor();

            services.ConfigureOptions<SwaggerGenSecurityOptions>();

            return services;
        }
        public static IApplicationBuilder UseDevelopment(this IApplicationBuilder app)
        {
            var securityConfiguration = app.ApplicationServices.GetRequiredService<ISecurityConfiguration>();
            string ResourceDisplayName = securityConfiguration.ResourceDisplayName;
            string ClientId = securityConfiguration.SwaggerDefaultClientId;
            string ClientSecret = securityConfiguration.SwaggerDefaultClientSecret;

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ResourceDisplayName);
                c.OAuthClientId(ClientId);
                c.OAuthClientSecret(ClientSecret);
                c.OAuthAppName("Identity Server");
            });

            return app;
        }
    }
}
