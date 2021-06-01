using RepoDb.Attributes;

namespace Domain.Identity.Data
{
    public class GlobalParameter
    {
        [Primary]
        public string ParameterID { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
