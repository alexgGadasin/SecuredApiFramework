using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ServiceDiscovery
{
    public interface IServiceDiscoveryRegistration
    {
        string ID { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        int Port { get; set; }
    }
}
