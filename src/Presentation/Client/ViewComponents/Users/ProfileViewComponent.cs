using Client.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Client.ViewComponents.Users
{
    public sealed class ProfileViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public ProfileViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _userService.ProfileAsync();
            return View(result);
        }
    }    
}