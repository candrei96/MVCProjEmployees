using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OA.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public IActionResult Index()
        {
            IExceptionHandlerPathFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;

            return PartialView("Error");
        }
    }
}