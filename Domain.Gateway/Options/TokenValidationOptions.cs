using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Gateway
{
    //public class TokenValidationOptions : IConfigureNamedOptions<JwtBearerOptions>
    //{
    //    private readonly IHttpClientFactory _httpClientFactory;
    //    private readonly AppSettings _appSettings;
    //    public SwaggerGenDocOptions(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
    //    {
    //        _httpClientFactory = httpClientFactory;
    //        _appSettings = appSettings.Value;
    //    }
    //    public void Configure(string name, SwaggerGenOptions options)
    //    {
    //        string ResourceDisplayName = _securityOptions.ResourceDisplayName;
    //        string ResourceVersion = _securityOptions.ResourceVersion;

    //        options.SwaggerDoc("v1", new OpenApiInfo { Title = ResourceDisplayName, Version = ResourceVersion });
    //    }
    //    public void Configure(SwaggerGenOptions options) => Configure(Options.DefaultName, options);
    //}
}
