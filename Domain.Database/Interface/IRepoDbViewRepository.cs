using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Database
{
    public interface IRepoDbViewRepository<T> where T : class
    {
        Task<IEnumerable<T>> ReadByQuery(string query, object parameter);
    }
}
