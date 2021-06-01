using Domain.Api;
using Domain.Database;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace Domain.Identity.Oauth
{
    public class AuthorizationCodeRepository : RepoSqlSrvDbRepository<AuthorizationCode>
    {
        public AuthorizationCodeRepository(IUnitOfWork uow) : base(uow) { }
        public async Task<AuthorizationCode> GenerateCode(string UserId, long CodeTokenExpiration)
        {
            var AuthCode = new AuthorizationCode()
            {
                Code = ApiHelper.GenerateRandomCryptographicKey(32),
                Expire = DateTime.Now.AddMinutes(CodeTokenExpiration),
                UserId = UserId
            };

            while(await base.Any(c => c.Code == AuthCode.Code))
                AuthCode.Code = ApiHelper.GenerateRandomCryptographicKey(32);

            await base.Insert(AuthCode);
            return AuthCode;
        }
        public async Task<bool> ValidateCode(AuthorizationCode AuthCode)
        {
            return await base.Any(c => c.Code == AuthCode.Code && c.Expire > DateTime.Now);
        }
        public async Task<string> FetchCode(AuthorizationCode AuthCode)
        {
            var currentCode = await base.ReadByLambda(c => c.Code == AuthCode.Code && c.Expire > DateTime.Now);

            if (!currentCode.Any())
                throw new ApiException("Code", ApiValidationType.NotFound);
            else
            {
                var UserId = currentCode.First().UserId;
                await base.Delete(currentCode.First());
                return UserId;
            }
        }
    }
}
