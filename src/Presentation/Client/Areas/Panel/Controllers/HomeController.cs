using Client.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Panel.Controllers
{
    [Area(AppConstants.PANELAREA)]
    [Authorize]
    public sealed class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
    }
}