using Domain.Database;

namespace Domain.Resource.General.Data
{
    public class CountryRepository : RepoSqlSrvDbRepository<Country>
    {
        public CountryRepository(IUnitOfWork uow) : base(uow) { }
    }
}
