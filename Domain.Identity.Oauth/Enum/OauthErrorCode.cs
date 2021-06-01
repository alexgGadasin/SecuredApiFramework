using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Identity.Oauth
{
    public enum OauthErrorCode
    {
        [Description("access_denied")]
        access_denied,
        [Description("invalid_client")]
        invalid_client,
        [Description("invalid_grant")]
        invalid_grant,
        [Description("invalid_request")]
        invalid_request,
        [Description("invalid_scope")]
        invalid_scope,
        [Description("invalid_token")]
        invalid_token,
        [Description("server_error")]
        server_error,
        [Description("temporarily_unavailable")]
        temporarily_unavailable,
        [Description("unauthorized_client")]
        unauthorized_client,
        [Description("unsupported_grant_type")]
        unsupported_grant_type,
        [Description("unsupported_response_type")]
        unsupported_response_type
    }
}
