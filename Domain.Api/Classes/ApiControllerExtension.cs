using Microsoft.AspNetCore.Mvc;

namespace Domain.Api
{
    public static class ApiControllerExtension
    {
        public static IActionResult SendResponse(this ControllerBase controller, IApiResponse apiResponse)
        {
            IActionResult response;

            controller.Response.StatusCode = (int)apiResponse.StatusCode;
            if (apiResponse.Body != null)
                response = new JsonResult(apiResponse.Body);
            else
                response = new JsonResult("");

            return response;
        }
        public static IActionResult SendResponse(this Controller controller, IApiResponse apiResponse)
        {
            IActionResult response;

            controller.Response.StatusCode = (int)apiResponse.StatusCode;
            if (apiResponse.Body != null)
                response = new JsonResult(apiResponse.Body);
            else
                response = new JsonResult("");

            return response;
        }
    }
}
