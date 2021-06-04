using Microsoft.Data.SqlClient;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Database
{
    public abstract class RepoSqlSrvDbRepository<T> : IRepoDbRepository<T> where T : class
    {
        private RepoSqlSrvDbUnitOfWork _uow;
        public string QuerySelect
        {
            get
            {
                return "SELECT * FROM [" + typeof(T).Name + "] WITH(NOLOCK) WHERE 1 = 1 ";
            }
        }
        public RepoSqlSrvDbRepository(IUnitOfWork uow)
        {
            this._uow = (RepoSqlSrvDbUnitOfWork)uow;
        }
        public async Task<bool> Any(Expression<Func<T, bool>> lambda)
        {
            return await CountByLambda(lambda) > 0;
        }
        public async Task<long> CountByLambda(Expression<Func<T, bool>> lambda)
        {
            return await _uow.Connection.CountAsync<T>(lambda);
        }
        public async Task<IEnumerable<T>> ReadByLambda(Expression<Func<T, bool>> lambda)
        {
            return await _uow.Connection.QueryAsync<T>(lambda);
        }
        public async Task<IEnumerable<T>> ReadAll()
        {
            return await _uow.Connection.QueryAllAsync<T>();
        }
        public async Task<IEnumerable<T>> ReadByQuery(string query, object parameters)
        {
            return await _uow.Connection.ExecuteQueryAsync<T>(query, parameters, transaction: _uow.Transaction);
        }
        public async Task Insert(T data)
        {
            await _uow.Connection.InsertAsync<T>(data, transaction: _uow.Transaction);
        }
        public async Task Insert(IEnumerable<T> data)
        {
            await _uow.Connection.InsertAllAsync<T>(data, transaction: _uow.Transaction);
        }
        public async Task Update(T data)
        {
            await _uow.Connection.UpdateAsync<T>(data, transaction: _uow.Transaction);
        }
        public async Task Delete(T data)
        {
            await _uow.Connection.DeleteAsync<T>(data, transaction: _uow.Transaction);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool stat)
        {
            if (stat)
            {
                //if (this._uow != null)
                //{
                //    this._uow.Dispose();
                //    this._uow = null;
                //}
            }
        }
    }
}
