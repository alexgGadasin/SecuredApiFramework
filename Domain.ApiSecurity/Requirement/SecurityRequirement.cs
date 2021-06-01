using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.ApiSecurity
{
    public class SecurityRequirement : IAuthorizationRequirement { }
}
