using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Domain.Api
{
    public class ApiResponse : IApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Body { get; set; }
        public ApiResponse(HttpStatusCode statusCode) : this(statusCode, null) { }
        public ApiResponse(HttpStatusCode statusCode, object result)
        {
            this.StatusCode = statusCode;
            this.Body = result;
        }
    }
}
