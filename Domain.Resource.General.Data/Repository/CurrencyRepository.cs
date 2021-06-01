using Domain.Database;

namespace Domain.Resource.General.Data
{
    public class CurrencyRepository : RepoSqlSrvDbRepository<Currency>
    {
        public CurrencyRepository(IUnitOfWork uow) : base(uow) { }
    }
}
