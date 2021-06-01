using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ApiSecurity
{
    public class SecurityConfiguration 
    {
        public string IdentityAddress { get; set; }
        public string ResourceName { get; set; }
        public string ResourceDisplayName { get; set; }
        public string ResourceVersion { get; set; }
        public string SwaggerDefaultClientId { get; set; }
        public string SwaggerDefaultClientSecret { get; set; }
    }
}
