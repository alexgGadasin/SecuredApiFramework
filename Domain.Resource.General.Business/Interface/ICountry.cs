using Domain.Api;
using Domain.Database;
using Domain.Resource.General.Data;
using System.Threading.Tasks;

namespace Domain.Resource.General.Business
{
    public interface ICountry : IBusinessLogic<Country>
    {
        Task<Country> GetByCode(string Code);
    }
}
