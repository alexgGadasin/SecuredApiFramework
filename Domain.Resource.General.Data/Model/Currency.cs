using RepoDb.Attributes;

namespace Domain.Resource.General.Data
{
    public class Currency
    {
        [Primary]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
