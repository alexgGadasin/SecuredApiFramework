using Domain.Database;

namespace Domain.Identity.Oauth
{
    public class ClientTypeRepository : RepoSqlSrvDbRepository<ClientType>
    {
        public ClientTypeRepository(IUnitOfWork uow) : base(uow) { }
    }
}
