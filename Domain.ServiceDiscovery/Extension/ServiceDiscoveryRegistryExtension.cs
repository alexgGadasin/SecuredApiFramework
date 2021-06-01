using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Domain.ServiceDiscovery
{
    public static class ServiceDiscoveryRegistryExtension
    {
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(
                p => new ConsulClient(consulConfig =>
                {
                    var address = p.GetRequiredService<IServiceDiscoveryConfiguration>().Address;
                    consulConfig.Address = new Uri(address);
                }
            ));

            return services;
        }
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services, Action<ServiceDiscoveryConfigurationData> configuration)
        {
            ServiceDiscoveryConfigurationData Data = new ServiceDiscoveryConfigurationData();
            configuration(Data);

            services.AddSingleton<IConsulClient, ConsulClient>(
                p => new ConsulClient(consulConfig =>
                {
                    consulConfig.Address = new Uri(Data.Address);
                }
            ));

            return services;
        }
        public static IApplicationBuilder UseServiceDiscovery(this IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            var registration = app.ApplicationServices.GetRequiredService<IServiceDiscoveryRegistration>();

            if (string.IsNullOrEmpty(registration.ID))
                throw new Exception("ID must be filled");
            else if (string.IsNullOrEmpty(registration.Name))
                throw new Exception("Name must be filled");
            else if (string.IsNullOrEmpty(registration.Address))
                throw new Exception("Address must be filled");
            else if (registration.Port == 0)
                throw new Exception("Port must be filled");
            else
            {
                AgentServiceRegistration agent = new AgentServiceRegistration()
                {
                    ID = registration.ID,
                    Name = registration.ID,
                    Address = registration.Address,
                    Port = registration.Port
                };

                logger.LogInformation("Registering with Consul");
                consulClient.Agent.ServiceDeregister(agent.ID).ConfigureAwait(true);
                consulClient.Agent.ServiceRegister(agent).ConfigureAwait(true);

                lifetime.ApplicationStopping.Register(() =>
                {
                    logger.LogInformation("Unregistering from Consul");
                });

                return app;
            }
        }
        public static IApplicationBuilder UseServiceDiscovery(this IApplicationBuilder app, Action<ServiceDiscoveryRegistrationData> configuration)
        {
            ServiceDiscoveryRegistrationData Data = new ServiceDiscoveryRegistrationData();
            configuration(Data);

            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            if (string.IsNullOrEmpty(Data.ID))
                throw new Exception("ID must be filled");
            else if (string.IsNullOrEmpty(Data.Name))
                throw new Exception("Name must be filled");
            else if (string.IsNullOrEmpty(Data.Address))
                throw new Exception("Address must be filled");
            else if (Data.Port == 0)
                throw new Exception("Port must be filled");
            else
            {
                AgentServiceRegistration agent = new AgentServiceRegistration()
                {
                    ID = Data.ID,
                    Name = Data.Name,
                    Address = Data.Address,
                    Port = Data.Port
                };

                logger.LogInformation("Registering with Consul");
                consulClient.Agent.ServiceDeregister(agent.ID).ConfigureAwait(true);
                consulClient.Agent.ServiceRegister(agent).ConfigureAwait(true);

                lifetime.ApplicationStopping.Register(() =>
                {
                    logger.LogInformation("Unregistering from Consul");
                });

                return app;
            }
        }
    }
}
