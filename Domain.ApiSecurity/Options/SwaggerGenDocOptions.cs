using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ApiSecurity
{
    public class SwaggerGenDocOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly ISecurityConfiguration _securityOptions;
        public SwaggerGenDocOptions(ISecurityConfiguration securityOptions)
        {
            _securityOptions = securityOptions;
        }
        public void Configure(string name, SwaggerGenOptions options)
        {
            string ResourceDisplayName = _securityOptions.ResourceDisplayName;
            string ResourceVersion = _securityOptions.ResourceVersion;

            options.SwaggerDoc("v1", new OpenApiInfo { Title = ResourceDisplayName, Version = ResourceVersion });
        }
        public void Configure(SwaggerGenOptions options) => Configure(Options.DefaultName, options);
    }
}
