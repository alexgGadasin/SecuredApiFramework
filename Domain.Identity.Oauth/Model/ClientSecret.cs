using RepoDb.Attributes;
using Domain.Database;
using System;

namespace Domain.Identity.Oauth
{
    public class ClientSecret : BaseModel
    {
        [Primary]
        public long ClientId { get; set; }
        public string Value { get; set; }
        public DateTime Expiration { get; set; }
    }
}
