using System;

namespace Domain.ServiceDiscovery
{
    public class ServiceDiscoveryRegistrationClient : IServiceDiscoveryRegistration
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public ServiceDiscoveryRegistrationClient(Action<ServiceDiscoveryRegistrationData> Configuration)
        {
            ServiceDiscoveryRegistrationData Data = new ServiceDiscoveryRegistrationData();
            Configuration(Data);

            this.ID = Data.ID;
            this.Name = Data.Name;
            this.Address = Data.Address;
            this.Port = Data.Port;
        }
    }
}
