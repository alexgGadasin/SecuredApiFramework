using Domain.Api;
using Domain.Resource.Protected.Business;
using Domain.Resource.Protected.Data;
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
    public class NewsController : Controller
    {
        private readonly string _label = "News";
        private readonly INews _news;
        public NewsController(IServiceProvider serviceProvider)
        {
            this._news = serviceProvider.GetRequiredService<INews>();
        }
        // Return all news in database and can be accessed by everyone
        [HttpGet, Route("")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var allowedRoles = new List<string>() { "reader", "writer", "supervisor" };

            if (!allowedRoles.Contains(HttpContext.Request.GetClaim("user.role")))
                return Unauthorized();
            else if(HttpContext.Request.GetClaim("user.role") == "writer")
            {
                var currentUser = HttpContext.Request.GetClaim("sub");
                var data = await _news.GetByUser(currentUser);
                var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
                var response = new ApiResponse(HttpStatusCode.OK, result);

                return this.SendResponse(response);
            }
            else
            {
                var data = await _news.GetAll();
                var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
                var response = new ApiResponse(HttpStatusCode.OK, result);

                return this.SendResponse(response);
            }
        }
        // Return single news in database and can be accessed by everyone
        [HttpGet, Route("{Id}")]
        public async Task<IActionResult> Get(long Id)
        {
            var allowedRoles = new List<string>() { "reader", "writer", "supervisor" };

            if (!allowedRoles.Contains(HttpContext.Request.GetClaim("user.role")))
                return Unauthorized();
            else
            {
                var data = await _news.GetById(Id);
                var result = new ApiResult(_label, ApiMessageType.SuccessfullyFetch, data);
                var response = new ApiResponse(HttpStatusCode.OK, result);

                return this.SendResponse(response);
            }
        }
        // write a news in database and can be accessed only by writer and supervisor
        [HttpPost, Route("insert")]
        public async Task<IActionResult> Insert(News data)
        {
            var allowedRoles = new List<string>() { "writer", "supervisor" };
            var currentUser = HttpContext.Request.GetClaim("sub");

            if (!allowedRoles.Contains(HttpContext.Request.GetClaim("user.role")))
                return Unauthorized();
            else
            {
                await _news.Insert(data, currentUser);
                var result = new ApiResult(_label, ApiMessageType.SuccessfullyWrite, data);
                var response = new ApiResponse(HttpStatusCode.OK, result);

                return this.SendResponse(response);
            }
        }
        // update a news in database and can be accessed only by writer and supervisor
        // writer can only update their own news while supervisor can access all news
        [HttpPost, Route("update")]
        public async Task<IActionResult> Update(News data)
        {
            var allowedRoles = new List<string>() { "writer", "supervisor" };
            var currentRole = HttpContext.Request.GetClaim("user.role");
            var currentUser = HttpContext.Request.GetClaim("sub");

            if (!allowedRoles.Contains(currentRole))
                return Unauthorized();
            else
            {
                var news = await _news.GetById(data.Id);

                if (currentRole != "supervisor" && news.CreatedBy != currentUser)
                    return BadRequest();
                else
                {
                    await _news.Update(data, currentUser);
                    var result = new ApiResult(_label, ApiMessageType.SuccessfullyUpdate, data);
                    var response = new ApiResponse(HttpStatusCode.OK, result);

                    return this.SendResponse(response);
                }
            }
        }
        // delete a news in database and can be accessed only by supervisor
        [HttpPost, Route("delete")]
        public async Task<IActionResult> Delete(News data)
        {
            var allowedRoles = new List<string>() { "supervisor" };
            var currentUser = HttpContext.Request.GetClaim("sub");

            if (!allowedRoles.Contains(HttpContext.Request.GetClaim("user.role")))
                return Unauthorized();
            else
            {
                await _news.Delete(data, currentUser);
                var result = new ApiResult(_label, ApiMessageType.SuccessfullyDelete, data);
                var response = new ApiResponse(HttpStatusCode.OK, result);

                return this.SendResponse(response);
            }
        }
    }
}
