using Domain.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Identity.Oauth
{
    public class ClientIdentityClaimRepository : RepoSqlSrvDbViewRepository<ClientIdentityClaim>
    {
        public ClientIdentityClaimRepository(IUnitOfWork uow) : base(uow) { }
        public async Task<IEnumerable<ClientIdentityClaim>> ReadByClient(long ClientId, List<string> scopes = null)
        {
            var parameters = new Dictionary<string, object>() {{ "ClientId", ClientId }};
            var scopeParam = "";
            if (scopes != null)
            {
                if (scopes.Any())
                {
                    scopeParam = "AND s.Name IN (";

                    for (int i = 0; i < scopes.Count; i++)
                    {
                        var paramName = "Scope" + (i + 1).ToString();
                        scopeParam += (scopeParam == "AND s.Name IN (" ? "" : ",") + "@" + paramName;
                        parameters.Add(paramName, scopes[i]);
                    }

                    scopeParam += ")";
                }
            }

            string query = string.Format(@"
                SELECT cs.ClientId, a.ResourceId, a.Name As ResourceName, s.ScopeId, s.Name As ScopeName
                FROM [ApiResourceScope] ars
	                INNER JOIN [ApiResource] a ON a.ResourceId = ars.ResourceId AND a.IsActive = 1
	                INNER JOIN [ApiScope] s ON s.ScopeId = ars.ScopeId AND s.IsActive = 1
	                INNER JOIN [ClientScope] cs ON cs.ScopeId = s.ScopeId AND cs.IsActive = 1
                WHERE ars.IsActive = 1 AND cs.ClientId = @ClientId {0}
                GROUP BY cs.ClientId, a.ResourceId, a.Name, s.ScopeId, s.Name", scopeParam);


            return await ReadByQuery(query: query, parameters: parameters);
        }
    }
}
