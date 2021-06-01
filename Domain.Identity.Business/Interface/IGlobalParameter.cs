using Domain.Api;
using Domain.Database;
using Domain.Identity.Data;
using Domain.Identity.Oauth;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public interface IGlobalParameter : IBusinessLogic<GlobalParameter>
    {
        Task<GlobalParameter> GetById(string ParameterID);
    }
}
