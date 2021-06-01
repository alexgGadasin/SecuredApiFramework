using System;

namespace Domain.ServiceDiscovery
{
    public class ServiceDiscoveryConfigurationClient : IServiceDiscoveryConfiguration
    {
        public string Address { get; private set; }
        public ServiceDiscoveryConfigurationClient(Action<ServiceDiscoveryConfigurationData> Configuration)
        {
            ServiceDiscoveryConfigurationData Data = new ServiceDiscoveryConfigurationData();
            Configuration(Data);

            this.Address = Data.Address;
        }
    }
}
