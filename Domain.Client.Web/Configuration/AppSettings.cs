using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Client.Web
{
    public class AppSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GatewayUrl { get; set; }
    }
}
