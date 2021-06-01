using System;

namespace Domain.ServiceDiscovery
{
    public interface IServiceDiscoveryConfiguration
    {
        string Address { get; }
    }
}
