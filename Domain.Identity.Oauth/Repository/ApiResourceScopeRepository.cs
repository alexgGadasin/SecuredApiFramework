using Domain.Database;

namespace Domain.Identity.Oauth
{
    public class ApiResourceScopeRepository : RepoSqlSrvDbRepository<ApiResourceScope>
    {
        public ApiResourceScopeRepository(IUnitOfWork uow) : base(uow) { }
    }
}
