using RepoDb.Attributes;

namespace Domain.Resource.Protected.Data
{
    public class Company
    {
        [Primary]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Market { get; set; }
    }
}
