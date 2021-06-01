using Domain.Api;
using Domain.Database;
using Domain.Resource.General.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Resource.General.Business
{
    public class CountryManager : BaseBusinessLogic, ICountry
    {
        public CountryManager(IUnitOfWork uow, IOptions<ConnectionStrings> connectionStrings) : base(uow, connectionStrings) { }
        public async Task<Country> GetByCode(string Code)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new CountryRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.Code == Code);

                        if (!result.Any())
                            throw new ApiException("Code", ApiValidationType.NotFound);
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
        public async Task<IEnumerable<Country>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new CountryRepository(_uow))
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
        public async Task Insert(Country data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new CountryRepository(_uow))
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
        public async Task Update(Country data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new CountryRepository(_uow))
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
        public async Task Delete(Country data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new CountryRepository(_uow))
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
