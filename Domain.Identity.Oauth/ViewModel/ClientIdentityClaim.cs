namespace Domain.Identity.Oauth
{
    public class ClientIdentityClaim
    {
        public long ClientId { get; set; }
        public long ResourceId { get; set; }
        public string ResourceName { get; set; }
        public long ScopeId { get; set; }
        public string ScopeName { get; set; }
    }
}
