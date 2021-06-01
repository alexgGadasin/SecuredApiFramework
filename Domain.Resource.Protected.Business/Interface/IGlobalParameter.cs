using Domain.Api;
using Domain.Database;
using Domain.Resource.Protected.Data;
using System.Threading.Tasks;

namespace Domain.Resource.Protected.Business
{
    public interface IGlobalParameter : IBusinessLogic<GlobalParameter>
    {
        Task<GlobalParameter> GetById(string ParameterId);
    }
}
