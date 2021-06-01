using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Gateway
{
    public class GatewayResourcesClient : IGatewayResources
    {
        public List<string> ApiResources { get; private set; }
        public GatewayResourcesClient(Action<GatewayResourcesData> Configuration)
        {
            GatewayResourcesData Data = new GatewayResourcesData();
            Configuration(Data);

            this.ApiResources = Data.ApiResources;
        }
    }
}
