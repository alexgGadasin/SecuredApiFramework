using Domain.Database;
using RepoDb.Attributes;
using System;

namespace Domain.Identity.Data
{
    public class Role : BaseModel
    {
        [Primary]
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
