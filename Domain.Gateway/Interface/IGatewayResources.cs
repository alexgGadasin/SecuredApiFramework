using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Gateway
{
    public interface IGatewayResources
    {
        List<string> ApiResources { get; }
    }
}
