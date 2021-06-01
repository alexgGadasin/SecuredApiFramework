using Domain.Database;

namespace Domain.Identity.Oauth
{ 
    public class ClientSecretRepository : RepoSqlSrvDbRepository<ClientSecret>
    {
        public ClientSecretRepository(IUnitOfWork uow) : base(uow) { }
    }
}
