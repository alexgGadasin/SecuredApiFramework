using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Api
{
    public class ApiError : IApiResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public List<ApiValidation> Validations { get; set; }
        public object Data { 
            get
            {
                return this.Validations;
            }
            set
            {
                this.Validations = (List<ApiValidation>)value;
            }
        }
        public ApiError() : this("", null) { }
        public ApiError(string message) : this(message, null) { }
        public ApiError(string message, List<ApiValidation> validations)
        {
            this.IsSuccess = false;
            this.Message = message;
            this.Validations = validations;
        }
        public ApiError(string key, ApiMessageType apiMessageType) : this(key, apiMessageType, null) { }
        public ApiError(string key, ApiMessageType apiMessageType, List<ApiValidation> validations)
        {
            this.IsSuccess = false;
            this.Message = ApiHelper.GetEnumDescription(apiMessageType, key);
            this.Validations = validations;
        }
    }
}
