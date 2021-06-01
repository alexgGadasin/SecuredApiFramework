using Domain.Database;

namespace Domain.Resource.Protected.Data
{
    public class CompanyRepository : RepoSqlSrvDbRepository<Company>
    {
        public CompanyRepository(IUnitOfWork uow) : base(uow) { }
    }
}
