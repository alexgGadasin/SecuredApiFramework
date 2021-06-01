using Domain.Database;

namespace Domain.Identity.Oauth
{
    public class ClientScopeRepository : RepoSqlSrvDbRepository<ClientScope>
    {
        public ClientScopeRepository(IUnitOfWork uow) : base(uow) { }
    }
}
