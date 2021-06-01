using Domain.Database;

namespace Domain.Identity.Oauth
{
    public class ApiScopeRepository : RepoSqlSrvDbRepository<ApiScope>
    {
        public ApiScopeRepository(IUnitOfWork uow) : base(uow) { }
    }
}
