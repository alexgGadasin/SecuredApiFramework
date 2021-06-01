namespace Domain.Identity.Data
{
    public class AuthenticateResponse
    {
        public string UserName { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public AuthenticateResponse(User user, string AccessToken, string RefreshToken)
        {
            this.UserName = user.UserName;
            this.ShortName = user.ShortName;
            this.FullName = user.FullName;
            this.AccessToken = AccessToken;
            this.RefreshToken = RefreshToken;
        }
    }
}
