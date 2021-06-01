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
    [Route("api/identity/apiresource")]
    public class ApiResourceController : Controller
    {
        private readonly string _label = "Api Resource";
        private readonly Business.IApiResource _apiResource;
        public ApiResourceController(IServiceProvider serviceProvider)
        {
            this._apiResource = serviceProvider.GetRequiredService<Business.IApiResource>();
        }
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var data = await _apiResource.GetAll();
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
        [Authorize]
        [HttpGet, Route("{ResourceId}")]
        public async Task<IActionResult> Get(long ResourceId)
        {
            var data = await _apiResource.GetById(ResourceId);
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
    }
}
