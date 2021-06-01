using Domain.Identity.Data;
using Domain.Database;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Identity.Business
{
    public class BaseBusinessLogic
    {
        protected readonly IUnitOfWork _uow;
        protected readonly AppSettings _appSettings;
        protected readonly ConnectionStrings _connectionStrings;
        protected ConnectionStrings ConnectionString
        {
            get
            {
                return _connectionStrings;
            }
        }
        public BaseBusinessLogic(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings)
        {
            this._uow = uow;
            this._appSettings = appSettings.Value;
            this._connectionStrings = connectionStrings.Value;
        }
    }
}
