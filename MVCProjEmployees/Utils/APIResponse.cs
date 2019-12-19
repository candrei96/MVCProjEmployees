using Microsoft.AspNetCore.Mvc;
using MVCProjEmployees.Utils;

namespace MVCProjEmployees.Models
{
    public static class APIResponse
    {
        public static JsonResult BadRequest()
        {
            JsonResult jsonResult = new JsonResult(Constants.BadRequestMessage)
            {
                StatusCode = 400,
                ContentType = "application/json"
            };

            return jsonResult;
        }
        public static JsonResult ApiNotFound()
        {
            JsonResult jsonResult = new JsonResult(Constants.NotFoundMessage)
            {
                StatusCode = 404,
                ContentType = "application/json"
            };

            return jsonResult;
        }

        public static JsonResult ApiConflict(string message)
        {
            JsonResult jsonResult = new JsonResult(message)
            {
                StatusCode = 409,
                ContentType = "application/json"
            };

            return jsonResult;
        }

        public static JsonResult DefaultErrorMessage(string message, int code)
        {
            JsonResult jsonResult = new JsonResult(message)
            {
                StatusCode = code,
                ContentType = "application/json"
            };

            return jsonResult;
        }
    }
}
