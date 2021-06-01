using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Database
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> ReadAll();
        Task<IEnumerable<T>> ReadByQuery(string query, object parameter);
        Task Insert(T data);
        Task Insert(IEnumerable<T> data);
        Task Delete(T data);
        Task Update(T data);
    }
}
