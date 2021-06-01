using Domain.Api;
using Domain.Database;
using Domain.Resource.General.Data;
using System.Threading.Tasks;

namespace Domain.Resource.General.Business
{
    public interface ICurrency : IBusinessLogic<Currency>
    {
        Task<Currency> GetByCode(string Code);
    }
}
