using Domain.Database;

namespace Domain.Identity.Oauth
{
    public class RefreshTokenRepository : RepoSqlSrvDbRepository<RefreshToken>
    {
        public RefreshTokenRepository(IUnitOfWork uow) : base(uow) { }
    }
}
