using Domain.Database;

namespace Domain.Resource.Protected.Data
{
    public class NewsRepository : RepoSqlSrvDbRepository<News>
    {
        public NewsRepository(IUnitOfWork uow) : base(uow) { }
    }
}
