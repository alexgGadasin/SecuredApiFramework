
using Domain.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Domain.Database
{
    public class RepoSqlSrvDbConnection : IConnection
    {
        private DateTime _connectionDate;
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; set; }
        public RepoSqlSrvDbConnection() {
            this._connectionDate = DateTime.MinValue;
        }
        public void OpenConnection(string connectionString)
        {
            try
            {
                this.Connection = new SqlConnection(connectionString);
                this.Connection.Open();
                this._connectionDate = DateTime.Now;
                SqlServerBootstrap.Initialize();
            }
            catch 
            {
                throw new Exception("Failed to establish connection");
            }
        }
        public DateTime GetDate()
        {
            return this._connectionDate;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool stat)
        {
            if (stat)
            {
                if (this.Transaction != null)
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
                if (this.Connection != null)
                {
                    this.Connection.Dispose();
                    this.Connection.Close();
                    this.Connection = null;
                }
            }
        }
    }
}
