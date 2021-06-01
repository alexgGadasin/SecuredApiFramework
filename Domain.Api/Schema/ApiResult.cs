using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace Domain.Api
{
    public class ApiResult : IApiResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ApiResult() : this("", null) { }
        public ApiResult(string message) : this(message, null) { }
        public ApiResult(string message, object data)
        {
            this.IsSuccess = true;
            this.Message = message;
            this.Data = data;
        }
        public ApiResult(string key, ApiMessageType apiMessageType) : this(key, apiMessageType, null) { }
        public ApiResult(string key, ApiMessageType apiMessageType, object data)
        {
            this.IsSuccess = true;
            this.Message = ApiHelper.GetEnumDescription(apiMessageType, key);
            this.Data = data;
        }
    }
}
