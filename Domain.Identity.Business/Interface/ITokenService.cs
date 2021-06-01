using Domain.Identity.Data;
using System;
using System.Security.Claims;

namespace Domain.Identity.Business
{
    public interface ITokenService : IDisposable
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipal(string token);
        ClaimsPrincipal Validate(string token);
        ClaimsPrincipal Validate(string token, string audience);
    }
}
