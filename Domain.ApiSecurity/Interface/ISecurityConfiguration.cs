using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ApiSecurity
{
    public interface ISecurityConfiguration
    {
        string IdentityAddress { get; set; }
        string ResourceName { get; set; }
        string ResourceDisplayName { get; set; }
        string ResourceVersion { get; set; }
        string SwaggerDefaultClientId { get; set; }
        string SwaggerDefaultClientSecret { get; set; }
    }
}
