using Domain.Database;
using RepoDb.Attributes;
using System;

namespace Domain.Identity.Oauth
{
    public class ApiResourceScope : BaseModel
    {
        [Primary]
        public long Id { get; set; }
        public long ResourceId { get; set; }
        public long ScopeId { get; set; }
    }
}
