using Domain.ApiSecurity;
using Domain.Api;
using Domain.Database;
using Domain.Identity.Business;
using Domain.ServiceDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Identity.Web
{
    public static class ServiceRegistryExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));
            services.ConfigureSwagger();

            return services;
        }
        public static IServiceCollection AddInstance(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWork, RepoSqlSrvDbUnitOfWork>();
            services.AddSingleton<IConnection, RepoSqlSrvDbConnection>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IOauthManager, OauthManager>();
            services.AddSingleton<IClient, ClientManager>();
            services.AddSingleton<IClientSecret, ClientSecretManager>();
            services.AddSingleton<IApiResource, ApiResourceManager>();
            services.AddSingleton<IApiScope, ApiScopeManager>();
            services.AddSingleton<IGlobalParameter, GlobalParameterManager>();

            // Supply parameters for swagger and security
            services.AddSingleton<ISecurityConfiguration, SecurityConfigurationClient>(
                p => new SecurityConfigurationClient(config =>
                {
                    var globalParameter = p.GetRequiredService<IGlobalParameter>().GetAll().Result;

                    config.IdentityAddress = globalParameter.Where(g => g.ParameterID == "IdentityAddress").First().Value;
                    config.ResourceName = globalParameter.Where(g => g.ParameterID == "ResourceName").First().Value;
                    config.ResourceDisplayName = globalParameter.Where(g => g.ParameterID == "ResourceDisplayName").First().Value;
                    config.ResourceVersion = globalParameter.Where(g => g.ParameterID == "ResourceVersion").First().Value;
                    config.SwaggerDefaultClientId = globalParameter.Where(g => g.ParameterID == "ClientId").First().Value;
                    config.SwaggerDefaultClientSecret = globalParameter.Where(g => g.ParameterID == "ClientSecret").First().Value;
                }));
            // Supply parameters for service discovery
            services.AddSingleton<IServiceDiscoveryConfiguration, ServiceDiscoveryConfigurationClient>(
                p => new ServiceDiscoveryConfigurationClient(config =>
                {
                    var globalParameter = p.GetRequiredService<IGlobalParameter>().GetAll().Result;

                    config.Address = globalParameter.Where(g => g.ParameterID == "ConsulAddress").First().Value;
                }));
            services.AddSingleton<IServiceDiscoveryRegistration, ServiceDiscoveryRegistrationClient>(
                p => new ServiceDiscoveryRegistrationClient(config =>
                {
                    var globalParameter = p.GetRequiredService<IGlobalParameter>().GetAll().Result;

                    config.ID = globalParameter.Where(g => g.ParameterID == "ResourceName").First().Value;
                    config.Name = globalParameter.Where(g => g.ParameterID == "ResourceDisplayName").First().Value;
                    config.Port = int.Parse(globalParameter.Where(g => g.ParameterID == "ResourcePort").First().Value);
                    config.Address = globalParameter.Where(g => g.ParameterID == "ResourceAddress").First().Value;
                }));

            return services;
        }
        public static IServiceCollection AddFilter(this IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.Filters.Add(new ApiExceptionFilter());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            return services;
        }
    }
}
