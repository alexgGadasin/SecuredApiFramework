using Domain.Api;
using Domain.Resource.General.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Resource.General.Web
{
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly string _label = "Currency";
        private readonly ICurrency _currency;
        public CurrencyController(IServiceProvider serviceProvider)
        {
            this._currency = serviceProvider.GetRequiredService<ICurrency>();
        }
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var data = await _currency.GetAll();
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
        [HttpGet, Route("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var data = await _currency.GetByCode(code);
            var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
            var response = new ApiResponse(HttpStatusCode.OK, result);

            return this.SendResponse(response);
        }
    }
}
