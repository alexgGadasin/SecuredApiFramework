using Domain.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Domain.Gateway
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder, clock) 
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization") && !Request.Headers.ContainsKey("X-Authorization"))
                return AuthenticateResult.Fail("Invalid Operation");
            else
            {
                var key = Request.Headers.ContainsKey("Authorization") ? "Authorization" : "X-Authorization";
                var auth = Request.Headers[key].FirstOrDefault();
                var method = auth.Split(" ").First();
                var token = auth.Split(" ").Last();
                var scheme = this.Scheme.Name;

                _client.DefaultRequestHeaders.Add("Authorization", $"{method} {token}");
                var response = await _client.GetAsync($"{Config.GatewayUrl}/api/connect/validate?access_token={token}&resource={scheme}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Claim> claims = new List<Claim>();
                    List<ApiClaim> tokenClaims = response.Content.ReadFromJsonAsync<List<ApiClaim>>().Result;
                    foreach (ApiClaim tokenClaim in tokenClaims)
                    {
                        claims.Add(new Claim(tokenClaim.Type, tokenClaim.Value));
                    }

                    var identity = new ClaimsIdentity(claims, scheme);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, scheme);
                    return AuthenticateResult.Success(ticket);
                } 
                else
                {
                    return AuthenticateResult.Fail("Invalid Operation");
                }
            }
        }
    }
}
