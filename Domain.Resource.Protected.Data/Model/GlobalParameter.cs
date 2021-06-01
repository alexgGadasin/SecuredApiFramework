using RepoDb.Attributes;

namespace Domain.Resource.Protected.Data
{
    public class GlobalParameter
    {
        [Primary]
        public string ParameterId { get; set; }
        public string Value { get; set; }
    }
}
