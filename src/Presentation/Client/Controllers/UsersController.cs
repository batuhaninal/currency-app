using Client.Models.Users;
using Client.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var result = await _userService.ProfileAsync();
            this.ShowResultMessage(result);
            return View(result.Data);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile([FromForm] UserProfileInput input)
        {
            if (!this.CheckModelStateValid(ModelState))
                return RedirectToAction(nameof(UsersController.Profile), "Users");

            bool willLogOut = false;

            if (!string.IsNullOrEmpty(input.OldPassword) && !string.IsNullOrEmpty(input.NewPassword))
                willLogOut = true;

            var result = await _userService.UpdateProfileAsync(input);

            _ = this.ShowResultMessage(result);

            if (!result.Success)
                willLogOut = false;


            if (willLogOut)
                return RedirectToAction(nameof(AuthController.LogoutUser), "Auth");

            return RedirectToAction(nameof(UsersController.Profile), "Users");
        }
    }
}