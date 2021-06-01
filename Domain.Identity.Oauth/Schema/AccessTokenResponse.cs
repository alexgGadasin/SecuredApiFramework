using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Identity.Oauth
{
    public class AccessTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
    }
}
