using Domain.Api;
using Domain.Database;
using Domain.Identity.Oauth;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public interface IClientSecret : IBusinessLogic<ClientSecret>
    {
        Task<ClientSecret> GetById(long Id);
    }
}
