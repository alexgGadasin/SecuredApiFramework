using Domain.Api;
using Domain.Resource.Protected.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Resource.Protected.Web
{
    [Route("api/[controller]")]
    public class UniversityController : Controller
    {
        private readonly string _label = "University";
        private readonly IUniversity _university;
        public UniversityController(IServiceProvider serviceProvider)
        {
            this._university = serviceProvider.GetRequiredService<IUniversity>();
        }
        [HttpGet, Route("")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var data = await _university.GetAll();
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
        [HttpGet, Route("{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
            var data = await _university.GetById(Id);
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
    }
}
