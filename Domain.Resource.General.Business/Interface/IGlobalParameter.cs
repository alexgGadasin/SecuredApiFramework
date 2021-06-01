using Domain.Api;
using Domain.Database;
using Domain.Resource.General.Data;
using System.Threading.Tasks;

namespace Domain.Resource.General.Business
{
    public interface IGlobalParameter : IBusinessLogic<GlobalParameter>
    {
        Task<GlobalParameter> GetById(string ParameterId);
    }
}
