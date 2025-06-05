using System.Net;
using Client.Models.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class ErrorsController : BaseController
    {
        [HttpGet]
        public IActionResult Index(int statusCode)
        {
            var method = HttpContext.Request.Method;
            string? redirectPath = string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase)
                ? HttpContext.Request.Headers["Referer"].ToString()
                : null;

            if (!string.IsNullOrEmpty(redirectPath))
            {
                if (redirectPath.ToLower().Contains("login"))
                    redirectPath = null;
            }

            return statusCode switch
                {
                    (int)HttpStatusCode.Forbidden => View("AccessDeniedPage", CustomErrorViewModel.AccessDenied(redirectPath)),
                    (int)HttpStatusCode.NotFound => View("NotFoundPage", CustomErrorViewModel.NotFound(redirectPath)),
                    _ => View("UnexpectedPage", CustomErrorViewModel.Internal(redirectPath))
                };
        }


        [HttpGet]
        public IActionResult AccessDeniedPage(string? redirectPath)
        {
            return View(CustomErrorViewModel.AccessDenied(redirectPath));
        }

        [HttpGet]
        public IActionResult NotFoundPage(string? redirectPath)
        {
            return View(CustomErrorViewModel.NotFound(redirectPath));
        }

        [HttpGet]
        public IActionResult UnexpectedPage(string? redirectPath)
        {
            return View(CustomErrorViewModel.Internal(redirectPath));
        }
    }
}