using Domain.Database;
using RepoDb.Attributes;
using System;

namespace Domain.Identity.Data
{
    public class UserClaim : BaseModel
    {
        [Primary]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
