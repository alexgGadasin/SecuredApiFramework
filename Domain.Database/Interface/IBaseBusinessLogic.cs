using Domain.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Database
{
    public interface IBaseBusinessLogic<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Insert(T data, string user);
        Task Delete(T data, string user);
        Task Update(T data, string user);
    }
}
