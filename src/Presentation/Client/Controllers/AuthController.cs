using Client.Models.Auth;
using Client.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(HttpContext, input);
                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message ?? "Beklenmeyen Hata!");
                    return View();
                }

                return RedirectToAction(nameof(HomeController.Index), "Home");

            }
            return View(input);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterInput input)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(HttpContext, input);
                if (result.Success)
                {
                    ModelState.AddModelError("", result.Message ?? "Beklenmeyen Hata!");
                    return View();
                }

                return RedirectToAction(nameof(HomeController.Index), "Home");

            }
            return View(input);
        }
    }    
}