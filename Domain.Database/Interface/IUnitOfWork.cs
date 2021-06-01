using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Database
{
    public interface IUnitOfWork : IConnection, IDisposable
    {
        IDbConnection Connection { get; }
        void BeginTransaction();
        void RollbackTransaction();
        void CommitTransaction();
    }
}
