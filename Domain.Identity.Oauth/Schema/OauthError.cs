using System.Text.Json.Serialization;
using Domain.Api;
using System.ComponentModel;

namespace Domain.Identity.Oauth
{
    public class OauthError
    {
        [JsonIgnore]
        public OauthErrorCode error_code { get; set; }
        [DefaultValue("")]
        public string error 
        { 
            get
            {
                return ApiHelper.GetEnumDescriptionAttribute(error_code).Description;
            }
        }
        public string error_description { get; set; }
        public string error_uri { get; set; }
        public OauthError(OauthErrorCode error_code) : this(error_code, null) { }
        public OauthError(OauthErrorCode error_code, string error_description) : this(error_code, error_description, null) { }
        public OauthError(OauthErrorCode error_code, string error_description, string error_uri)
        {
            this.error_code = error_code;
            this.error_description = error_description;
            this.error_uri = error_uri;
        }
    }
}
