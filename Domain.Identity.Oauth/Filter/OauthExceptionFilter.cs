using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Domain.Identity.Oauth
{
    public class OauthExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {

            if (context.Exception is OauthException)
            {
                OauthException ex = context.Exception as OauthException;
                OauthError apiError = new OauthError(ex.ErrorCode, ex.ErrorDescription, ex.ErrorUri);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(apiError);
            }
            else
            {
                OauthError apiError = new OauthError(OauthErrorCode.server_error);

                context.HttpContext.Response.StatusCode = 500;
                context.Result = new JsonResult(apiError);
            }

            base.OnException(context);
        }
    }
}
