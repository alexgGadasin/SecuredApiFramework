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
    public class UserManager : BaseBusinessLogic, IUser
    {
        public UserManager(IUnitOfWork uow, IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) : base(uow, appSettings, connectionStrings) { }
        public async Task<User> GetById(string UserId)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);
                
                using (_uow)
                {
                    using (var repo = new UserRepository(_uow))
                    {
                        var result = await repo.ReadByLambda(b => b.UserId == UserId);

                        if (!result.Any())
                            throw new ApiException("User", ApiValidationType.NotFound);
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
        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new UserRepository(_uow))
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
        public async Task Insert(User data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new UserRepository(_uow))
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
        public async Task Update(User data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new UserRepository(_uow))
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
        public async Task Delete(User data)
        {
            try
            {
                _uow.OpenConnection(ConnectionString.DefaultConnection);

                using (_uow)
                {
                    using (var repo = new UserRepository(_uow))
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
        public async Task RegisterUser(User data)
        {
            var response = new ApiResult();
            var validations = new List<ApiValidation>();

            _uow.OpenConnection(ConnectionString.DefaultConnection);

            using (_uow)
            {
                using (var userRepo = new UserRepository(_uow))
                {
                    try
                    {
                        bool IsExistsUser = await userRepo.Any(u => u.UserName == data.UserName);

                        if (IsExistsUser)
                            throw new ApiException("Username", ApiValidationType.AlreadyExist);
                        else
                        {
                            var LengthRandomString = Convert.ToInt32(_appSettings.LengthRandomString);
                            var SaltSize = Convert.ToInt32(_appSettings.SaltSize);

                            data.UserId = ApiHelper.GeneratedID(LengthRandomString, data.UserName);
                            data.SaltText = ApiHelper.GenerateRandomCryptographicKey(SaltSize);
                            data.Password = ApiHelper.HashWithSalt(data.Password, data.SaltText, SHA384.Create());
                            data.PasswordExpiredDate = _uow.GetDate().AddMonths(6);
                            data.PasswordErrorCounter = 0;
                            data.IsResetPassword = false;
                            data.IsLocked = false;
                            data.IsActive = true;
                            data.CreatedBy = data.UserId;
                            data.CreatedDate = _uow.GetDate();
                            data.ModifiedBy = null;
                            data.ModifiedDate = null;

                            _uow.BeginTransaction();
                            await userRepo.Insert(data);
                            _uow.CommitTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        _uow.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }
    }
}
