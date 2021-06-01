using Domain.Database;
using RepoDb.Attributes;
using System;

namespace Domain.Identity.Oauth
{
    public class Client : BaseModel
    {
        [Primary]
        public long ClientId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public string RedirectUri { get; set; }
    }
}
