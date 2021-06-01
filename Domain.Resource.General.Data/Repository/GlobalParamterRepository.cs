using Domain.Database;

namespace Domain.Resource.General.Data
{
    public class GlobalParameterRepository : RepoSqlSrvDbRepository<GlobalParameter>
    {
        public GlobalParameterRepository(IUnitOfWork uow) : base(uow) { }
    }
}
