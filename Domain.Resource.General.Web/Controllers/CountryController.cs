using Domain.Api;
using Domain.Resource.General.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Resource.General.Web
{
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private readonly string _label = "Country";
        private readonly ICountry _country;
        public CountryController(IServiceProvider serviceProvider)
        {
            this._country = serviceProvider.GetRequiredService<ICountry>();
        }
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var data = await _country.GetAll();
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
        [HttpGet, Route("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var data = await _country.GetByCode(code);
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
    }
}
