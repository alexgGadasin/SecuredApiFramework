using Domain.ApiSecurity;
using Domain.Api;
using Domain.Database;
using Domain.Resource.General.Business;
using Domain.ServiceDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Domain.Resource.General.Web
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
            services.AddSingleton<ICountry, CountryManager>();
            services.AddSingleton<ICurrency, CurrencyManager>();
            services.AddSingleton<IGlobalParameter, GlobalParameterManager>();

            // Supply parameters for swagger and security
            services.AddSingleton<ISecurityConfiguration, SecurityConfigurationClient>(
                p => new SecurityConfigurationClient(config =>
                {
                    var globalParameter = p.GetRequiredService<IGlobalParameter>().GetAll().Result;

                    config.IdentityAddress = globalParameter.Where(g => g.ParameterId == "IdentityAddress").First().Value;
                    config.ResourceName = globalParameter.Where(g => g.ParameterId == "ResourceName").First().Value;
                    config.ResourceDisplayName = globalParameter.Where(g => g.ParameterId == "ResourceDisplayName").First().Value;
                    config.ResourceVersion = globalParameter.Where(g => g.ParameterId == "ResourceVersion").First().Value;
                    config.SwaggerDefaultClientId = globalParameter.Where(g => g.ParameterId == "ClientId").First().Value;
                    config.SwaggerDefaultClientSecret = globalParameter.Where(g => g.ParameterId == "ClientSecret").First().Value;
                }));
            // Supply parameters for service discovery
            services.AddSingleton<IServiceDiscoveryConfiguration, ServiceDiscoveryConfigurationClient>(
                p => new ServiceDiscoveryConfigurationClient(config =>
                {
                    var globalParameter = p.GetRequiredService<IGlobalParameter>().GetAll().Result;

                    config.Address = globalParameter.Where(g => g.ParameterId == "ConsulAddress").First().Value;
                }));
            services.AddSingleton<IServiceDiscoveryRegistration, ServiceDiscoveryRegistrationClient>(
                p => new ServiceDiscoveryRegistrationClient(config =>
                {
                    var globalParameter = p.GetRequiredService<IGlobalParameter>().GetAll().Result;

                    config.ID = globalParameter.Where(g => g.ParameterId == "ResourceName").First().Value;
                    config.Name = globalParameter.Where(g => g.ParameterId == "ResourceDisplayName").First().Value;
                    config.Port = int.Parse(globalParameter.Where(g => g.ParameterId == "ResourcePort").First().Value);
                    config.Address = globalParameter.Where(g => g.ParameterId == "ResourceAddress").First().Value;
                }));

            return services;
        }
        public static IServiceCollection AddFilter(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            });

            return services;
        }
    }
}
