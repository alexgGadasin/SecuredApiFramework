using Domain.Database;
using Domain.Identity.Oauth;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public interface IApiScope : IBusinessLogic<ApiScope>
    {
        Task<ApiScope> GetById(long ScopeId);
    }
}
