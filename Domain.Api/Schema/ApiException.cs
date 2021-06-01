using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Domain.Api
{
    public class ApiException : Exception
    {
        public string ExceptionMessage { get; set; }
        public List<ApiValidation> Validations { get; set; }
        public ApiException() : base() { }
        public ApiException(List<ApiValidation> validations) : base()
        {
            this.Validations = validations;
        }
        public ApiException(string key, ApiValidationType validation) : base()
        {
            this.Validations = new List<ApiValidation>();
            this.AddValidation(key, validation);
        }
        public ApiException(List<ApiValidation> validations, string message) : base(message)
        {
            this.Validations = validations;
        }
        public ApiException(string key, ApiValidationType validation, string message) : base(message)
        {
            this.Validations = new List<ApiValidation>();
            this.AddValidation(key, validation);
        }
        public ApiException(List<ApiValidation> validations, string message, Exception innerException) : base(message, innerException)
        {
            this.Validations = validations;
        }
        public ApiException(string key, ApiValidationType validation, string message, Exception innerException) : base(message, innerException)
        {
            this.Validations = new List<ApiValidation>();
            this.AddValidation(key, validation);
        }
        public ApiException(List<ApiValidation> validations, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Validations = validations;
        }
        public ApiException(string key, ApiValidationType validation, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Validations = new List<ApiValidation>();
            this.AddValidation(key, validation);
        }
        public void AddValidation(string key, ApiValidationType validation)
        {
            this.Validations.Add(new ApiValidation()
            {
                Key = key,
                ValidationType = validation
            });
        }
    }
}
