using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ServiceDiscovery
{
    public class ServiceDiscoveryRegistrationData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
    }
}
