namespace Domain.Identity.Business
{
    public class AppSettings
    {
        public string Version { get; set; }
        public string AppUrl { get; set; }
        public string PassPhrase { get; set; }
        public string HashAlgorithm { get; set; }
        public string PasswordIterations { get; set; }
        public string InitVector { get; set; }
        public string KeySize { get; set; }
        public string SaltFixed { get; set; }
        public string LengthRandomString { get; set; }
        public string TokenLength { get; set; }
        public string SecuredTokenLength { get; set; }
        public string SaltSize { get; set; }
        public long CodeTokenExpiration { get; set; }
        public long AccessTokenExpiration { get; set; }
        public long RefreshTokenExpiration { get; set; }
    }
}
