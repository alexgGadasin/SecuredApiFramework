using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ApiSecurity
{
    public class SecurityRequirementHandler : AuthorizationHandler<SecurityRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;
        private readonly ISecurityConfiguration _securityOptions;

        public SecurityRequirementHandler(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ISecurityConfiguration securityOptions)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
            _securityOptions = securityOptions;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SecurityRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];
                var identityAddress = _securityOptions.IdentityAddress;

                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await _client.GetAsync($"{identityAddress}/api/connect/validate?access_token={accessToken}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
