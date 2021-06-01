using Domain.Database;

namespace Domain.Identity.Oauth
{
    public class ClientRepository : RepoSqlSrvDbRepository<Client>
    {
        public ClientRepository(IUnitOfWork uow) : base(uow) { }
    }
}
