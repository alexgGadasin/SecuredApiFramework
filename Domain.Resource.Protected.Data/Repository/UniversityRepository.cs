using Domain.Database;

namespace Domain.Resource.Protected.Data
{
    public class UniversityRepository : RepoSqlSrvDbRepository<University>
    {
        public UniversityRepository(IUnitOfWork uow) : base(uow) { }
    }
}
