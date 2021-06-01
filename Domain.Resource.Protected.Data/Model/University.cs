using RepoDb.Attributes;

namespace Domain.Resource.Protected.Data
{
    public class University
    {
        [Primary]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
