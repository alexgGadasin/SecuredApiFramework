using Domain.Api;
using Domain.Database;
using Domain.Resource.Protected.Data;
using System.Threading.Tasks;

namespace Domain.Resource.Protected.Business
{
    public interface ICompany : IBusinessLogic<Company>
    {
        Task<Company> GetById(long Id);
    }
}
