using Domain.Identity.Data;
using Domain.Identity.Oauth;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public interface IOauthManager
    {
        Task<string> Authorize(AuthorizationRequest data);
        Task<AuthorizationResponse> Authorize(AuthenticateRequest data, string state);
        Task<AccessTokenResponse> Token(AccessTokenRequest data);
    }
}
