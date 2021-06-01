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
    public class ClientManager : BaseBusinessLogic, IClient
    {
        public ClientManager(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) : base(uow, appSettings, connectionStrings) { }
        public async Task<Client> GetById(long ClientId)
        {
            try
            {
                ApiResult apiResult = new ApiResult();
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.ClientId == ClientId);
                                       
                        if (!result.Any())
                            throw new ApiException("Client", ApiValidationType.NotFound);
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
        public async Task<Client> GetByName(string Name)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.Name == Name && b.IsActive == true);

                        if (!result.Any())
                            throw new ApiException("Client", ApiValidationType.NotFound);
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
        public async Task<IEnumerable<Client>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
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
        public async Task Insert(Client data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
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
        public async Task Update(Client data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
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
        public async Task Delete(Client data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new ClientRepository(_uow))
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
