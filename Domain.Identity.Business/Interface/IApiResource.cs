using Domain.Identity.Oauth;
using Domain.Database;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public interface IApiResource : IBusinessLogic<ApiResource>
    {
        Task<ApiResource> GetById(long ResourceId);
    }
}
