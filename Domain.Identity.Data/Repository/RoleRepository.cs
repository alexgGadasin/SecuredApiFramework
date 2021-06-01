using Domain.Database;

namespace Domain.Identity.Data
{
    public class RoleRepository : RepoSqlSrvDbRepository<Role>
    {
        public RoleRepository(IUnitOfWork uow) : base(uow) { }
    }
}