using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Database
{
    public interface IConnection : IDisposable
    {
        DateTime GetDate();
        void OpenConnection(string connectionString);
    }
}
