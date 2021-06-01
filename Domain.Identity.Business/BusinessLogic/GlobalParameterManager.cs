using Domain.Api;
using Domain.Identity.Data;
using Domain.Database;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Domain.Identity.Business
{
    public class GlobalParameterManager : BaseBusinessLogic, IGlobalParameter
    {
        public GlobalParameterManager(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) : base(uow, appSettings, connectionStrings) { }
        public async Task<GlobalParameter> GetById(string ParameterID)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new GlobalParameterRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.ParameterID == ParameterID);

                        if (!result.Any())
                            throw new ApiException("GlobalParameter", ApiValidationType.NotFound);
                        else
                            return result.First();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GlobalParameter>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new GlobalParameterRepository(_uow))
                    {
                        return await repo.ReadAll();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Insert(GlobalParameter data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new GlobalParameterRepository(_uow))
                    {
                        _uow.BeginTransaction();
                        await repo.Insert(data);
                        _uow.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Update(GlobalParameter data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new GlobalParameterRepository(_uow))
                    {
                        _uow.BeginTransaction();
                        await repo.Update(data);
                        _uow.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Delete(GlobalParameter data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new GlobalParameterRepository(_uow))
                    {
                        _uow.BeginTransaction();
                        await repo.Insert(data);
                        _uow.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
