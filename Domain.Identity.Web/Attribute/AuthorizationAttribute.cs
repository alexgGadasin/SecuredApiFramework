using Domain.Api;
using Domain.Identity.Oauth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Identity.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["Sub"] == null)
            {
                OauthError apiError = null;

                if (context.HttpContext.Items["Err"] != null)
                {
                    string errCode = context.HttpContext.Items["Err"].ToString();
                    string errMessage = context.HttpContext.Items["ErrMsg"] == null ? "" : context.HttpContext.Items["ErrMsg"].ToString(); 

                    if (errCode == "IDX10223")
                        apiError = new OauthError(OauthErrorCode.invalid_token, errMessage);
                }

                if (apiError == null)
                {
                    context.Result = new JsonResult(
                        new { message = ApiHelper.GetEnumDescription(ApiMessageType.NotAuthorized) }
                    ){ StatusCode = StatusCodes.Status401Unauthorized };
                } 
                else
                {
                    context.Result = new JsonResult(apiError) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
    }
}
