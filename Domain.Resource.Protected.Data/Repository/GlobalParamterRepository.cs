using Domain.Database;

namespace Domain.Resource.Protected.Data
{
    public class GlobalParameterRepository : RepoSqlSrvDbRepository<GlobalParameter>
    {
        public GlobalParameterRepository(IUnitOfWork uow) : base(uow) { }
    }
}
