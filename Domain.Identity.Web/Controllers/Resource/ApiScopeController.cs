using Domain.Api;
using Domain.Identity.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Identity.Web
{
    [Route("api/identity/apiscope")]
    public class ApiScopeController : Controller
    {
        private readonly string _label = "Api Scope";
        private readonly Business.IApiScope _apiScope;
        public ApiScopeController(IServiceProvider serviceProvider)
        {
            this._apiScope = serviceProvider.GetRequiredService<Business.IApiScope>();
        }
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var data = await _apiScope.GetAll();
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
        [HttpGet, Route("{ScopeId}")]
        public async Task<IActionResult> Get(long ScopeId)
        {
            var data = await _apiScope.GetById(ScopeId);
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
    }
}
