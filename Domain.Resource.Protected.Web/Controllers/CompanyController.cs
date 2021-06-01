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
    public class CompanyController : Controller
    {
        private readonly string _label = "Company";
        private readonly ICompany _company;
        public CompanyController(IServiceProvider serviceProvider)
        {
            this._company = serviceProvider.GetRequiredService<ICompany>();
        }
        [HttpGet, Route("")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var data = await _company.GetAll();
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
        [HttpGet, Route("{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
            var data = await _company.GetById(Id);
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
    }
}
