using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Database
{
    public class RepoSqlSrvDbUnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        private readonly RepoSqlSrvDbConnection _conn;
        public IDbConnection Connection
        {
            get
            {
                return _conn.Connection;
            }
        }
        public IDbTransaction Transaction
        {
            get
            {
                return _conn.Transaction;
            }
        }
        public RepoSqlSrvDbUnitOfWork(IConnection conn) 
        {
            this._conn = (RepoSqlSrvDbConnection)conn; 
        }
        public void OpenConnection(string connectionString)
        {
            _conn.OpenConnection(connectionString);
        }
        public void BeginTransaction() 
        {
            _conn.Transaction = _conn.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }
        public void CommitTransaction()
        {
            if (_conn.Transaction != null)
                _conn.Transaction.Commit();
        }
        public void RollbackTransaction()
        {
            if (_conn.Transaction != null)
                _conn.Transaction.Rollback();
        }
        protected override void Dispose(bool stat)
        {
            if (stat)
            {
                if (_conn != null)
                {
                    _conn.Dispose();
                }
                base.Dispose(stat);
            }
        }
        public DateTime GetDate()
        {
            return _conn.GetDate();
        }
    }
}
