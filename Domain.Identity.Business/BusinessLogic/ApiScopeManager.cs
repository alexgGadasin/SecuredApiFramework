using Domain.Api;
using Domain.Identity.Data;
using Domain.Database;
using Domain.Identity.Oauth;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public class ApiScopeManager : BaseBusinessLogic, IApiScope
    {
        public ApiScopeManager(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) : base(uow, appSettings, connectionStrings) { }
        public async Task<ApiScope> GetById(long ScopeId)
        {
            try
            {
                ApiResult apiResult = new ApiResult();
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ApiScopeRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.ScopeId == ScopeId);

                        if (!result.Any())
                            throw new ApiException("ApiScope", ApiValidationType.NotFound);
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
        public async Task<IEnumerable<ApiScope>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ApiScopeRepository(_uow))
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
        public async Task Insert(ApiScope data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ApiScopeRepository(_uow))
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
        public async Task Update(ApiScope data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ApiScopeRepository(_uow))
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
        public async Task Delete(ApiScope data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ApiScopeRepository(_uow))
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
