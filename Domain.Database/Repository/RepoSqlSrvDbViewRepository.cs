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
    public abstract class RepoSqlSrvDbViewRepository<T> : IRepoDbViewRepository<T> where T : class
    {
        private RepoSqlSrvDbUnitOfWork _uow;
        public RepoSqlSrvDbViewRepository(IUnitOfWork uow)
        {
            this._uow = (RepoSqlSrvDbUnitOfWork)uow;
        }
        public async Task<IEnumerable<T>> ReadByQuery(string query, object parameters)
        {
            return await _uow.Connection.ExecuteQueryAsync<T>(query, parameters, transaction: _uow.Transaction);
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
                if (this._uow != null)
                {
                    this._uow.Dispose();
                    this._uow = null;
                }
            }
        }
    }
}
