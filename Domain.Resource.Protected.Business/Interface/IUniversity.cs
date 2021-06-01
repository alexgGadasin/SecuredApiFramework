using Domain.Api;
using Domain.Database;
using Domain.Resource.Protected.Data;
using System.Threading.Tasks;

namespace Domain.Resource.Protected.Business
{
    public interface IUniversity : IBusinessLogic<University>
    {
        Task<University> GetById(long Id);
    }
}
