using Domain.Database;

namespace Domain.Identity.Data
{
    public class UserClaimRepository : RepoSqlSrvDbRepository<UserClaim>
    {
        public UserClaimRepository(IUnitOfWork uow) : base(uow) { }
    }
}