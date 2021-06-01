using Domain.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Database
{
    public interface IBusinessLogic<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Insert(T data);
        Task Delete(T data);
        Task Update(T data);
    }
}
