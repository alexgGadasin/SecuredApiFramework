using Domain.Api;
using Domain.Database;
using Domain.Resource.Protected.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Resource.Protected.Business
{
    public class NewsManager : BaseBusinessLogic, INews
    {
        public NewsManager(IUnitOfWork uow, IOptions<ConnectionStrings> connectionStrings) : base(uow, connectionStrings) { }
        public async Task<News> GetById(long Id)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    List<string> validations = new List<string>();

                    using (var repo = new NewsRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(d => d.Id == Id && d.IsActive == true);

                        if (!result.Any())
                            throw new ApiException("News", ApiValidationType.NotFound);
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
        public async Task<IEnumerable<News>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new NewsRepository(_uow))
                    {
                        return await repo.ReadByLambda(d => d.IsActive == true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<News>> GetByUser(string user)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new NewsRepository(_uow))
                    {
                        return await repo.ReadByLambda(d => d.CreatedBy == user && d.IsActive == true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Insert(News data, string user)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new NewsRepository(_uow))
                    {
                        _uow.BeginTransaction();

                        data.IsActive = true;
                        data.CreatedBy = user;
                        data.CreatedDate = DateTime.Now;
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
        public async Task Update(News data, string user)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new NewsRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(d => d.Id == data.Id && d.IsActive == true);

                        if(!result.Any())
                            throw new ApiException("News", ApiValidationType.NotFound);
                        else
                        {
                            var currentData = result.First();

                            _uow.BeginTransaction();

                            data.IsActive = true;
                            data.CreatedBy = currentData.CreatedBy;
                            data.CreatedDate = currentData.CreatedDate;
                            data.ModifiedBy = user;
                            data.ModifiedDate = DateTime.Now;
                            await repo.Update(data);

                            _uow.CommitTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Delete(News data, string user)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new NewsRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(d => d.Id == data.Id && d.IsActive == true);

                        if (!result.Any())
                            throw new ApiException("News", ApiValidationType.NotFound);
                        else
                        {
                            var currentData = result.First();

                            _uow.BeginTransaction();

                            currentData.IsActive = false;
                            currentData.ModifiedBy = user;
                            currentData.ModifiedDate = DateTime.Now;
                            await repo.Update(currentData);

                            _uow.CommitTransaction();
                        }
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
