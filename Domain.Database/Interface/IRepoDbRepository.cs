using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Database
{
    public interface IRepoDbRepository<T> : IRepository<T> where T : class
    {
        Task<bool> Any(Expression<Func<T, bool>> lambda);
        Task<long> CountByLambda(Expression<Func<T, bool>> lambda);
        Task<IEnumerable<T>> ReadByLambda(Expression<Func<T, bool>> lambda);
    }
}
