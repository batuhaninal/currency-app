using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Client.Attributes;

namespace Client.Controllers;

[Authorize]
public class HomeController : Controller
{
    [Breadcrumb("Ana Sayfa")]
    public IActionResult Index()
    {
        if (HttpContext.User.IsInRole("admin"))
            return RedirectToAction(nameof(Client.Areas.Panel.Controllers.CurrenciesController.Index), "Currencies", new { area = "panel" });
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
