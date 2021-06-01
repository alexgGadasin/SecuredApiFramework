using Domain.Api;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Domain.Identity.Oauth
{
    public class OauthException : Exception
    {
        public OauthErrorCode ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorUri { get; set; }
        public OauthException() : base() { }
        public OauthException(OauthErrorCode errorCode) : this(errorCode, (string)null) { }
        public OauthException(OauthErrorCode errorCode, string errorDescription) : this(errorCode, errorDescription, (string)null) { }
        public OauthException(OauthErrorCode errorCode, string errorDescription, string errorUri) : base(OauthErrorMessage(errorCode))
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }
        public OauthException(OauthErrorCode errorCode, Exception innerException) : this(errorCode, null, innerException) { }
        public OauthException(OauthErrorCode errorCode, string errorDescription, Exception innerException) : this(errorCode, errorDescription, null, innerException) { }
        public OauthException(OauthErrorCode errorCode, string errorDescription, string errorUri, Exception innerException) : base(OauthErrorMessage(errorCode), innerException)
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }
        private static string OauthErrorMessage(OauthErrorCode errorCode)
        {
            return ApiHelper.GetEnumDescriptionAttribute(errorCode).Description;
        }
    }
}
