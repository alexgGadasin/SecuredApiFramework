using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Api;
using Domain.Database;

namespace Domain.Identity.Data
{
    public class GlobalParameterRepository : RepoSqlSrvDbRepository<GlobalParameter>
    {
        public GlobalParameterRepository(IUnitOfWork uow) : base(uow) { }
        public async Task<string> GetValue(string ParameterId)
        {
            var response = await base.ReadByLambda(g => g.ParameterID == ParameterId);
            
            if (!response.Any())
                throw new ApiException("Global Parameter", ApiValidationType.NotFound);
            else
                return response.First().Value;
        }
    }
}