namespace Domain.Api
{
    public class ApiClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public ApiClaim() { }
        public ApiClaim(string Type, string Value)
        {
            this.Type = Type;
            this.Value = Value;
        }
    }
}
