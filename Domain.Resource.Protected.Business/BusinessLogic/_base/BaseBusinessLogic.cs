using Domain.Resource.Protected.Data;
using Domain.Database;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Resource.Protected.Business
{
    public class BaseBusinessLogic
    {
        protected readonly IUnitOfWork _uow;
        protected readonly ConnectionStrings _connectionStrings;
        protected ConnectionStrings ConnectionString
        {
            get
            {
                return _connectionStrings;
            }
        }
        public BaseBusinessLogic(IUnitOfWork uow, IOptions<ConnectionStrings> connectionStrings)
        {
            this._uow = uow;
            this._connectionStrings = connectionStrings.Value;
        }
    }
}
