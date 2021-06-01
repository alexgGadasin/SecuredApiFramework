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
    public class ClientSecretManager : BaseBusinessLogic, IClientSecret
    {
        public ClientSecretManager(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) : base(uow, appSettings, connectionStrings) { }
        public async Task<ClientSecret> GetById(long ClientId)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientSecretRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.ClientId == ClientId);

                        if (!result.Any())
                            throw new ApiException("ClientSecret", ApiValidationType.NotFound);
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
        public async Task<IEnumerable<ClientSecret>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientSecretRepository(_uow))
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
        public async Task Insert(ClientSecret data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientSecretRepository(_uow))
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
        public async Task Update(ClientSecret data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientSecretRepository(_uow))
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
        public async Task Delete(ClientSecret data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientSecretRepository(_uow))
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
