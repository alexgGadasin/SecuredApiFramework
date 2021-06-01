using Domain.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Identity.Oauth
{
    public class ApiResourceRepository : RepoSqlSrvDbRepository<ApiResource>
    {
        public ApiResourceRepository(IUnitOfWork uow) : base(uow) { }
        public async Task<IEnumerable<ApiResource>> ReadByClient(long ClientId)
        {
            string query = @"
                SELECT a.ResourceId, a.Name, a.DisplayName, a.Description, a.IsActive, a.CreatedBy, a.CreatedDate, a.ModifiedBy, a.ModifiedDate
                FROM [ApiResourceScope] ars
	                INNER JOIN [ApiResource] a ON a.ResourceId = ars.ResourceId AND a.IsActive = 1
	                INNER JOIN [ApiScope] s ON s.ScopeId = ars.ScopeId AND s.IsActive = 1
	                INNER JOIN [ClientScope] cs ON cs.ScopeId = s.ScopeId AND cs.IsActive = 1
                WHERE ars.IsActive = 1 AND cs.ClientId = @ClientId
                GROUP BY a.ResourceId, a.Name, a.DisplayName, a.Description, a.IsActive, a.CreatedBy, a.CreatedDate, a.ModifiedBy, a.ModifiedDate";

            var parameters = new Dictionary<string, object>
            {
                { "ClientId", ClientId }
            };

            return await base.ReadByQuery(query: query, parameters: parameters);
        }
    }
}
