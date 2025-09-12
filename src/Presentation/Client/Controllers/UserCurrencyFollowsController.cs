using Client.Models.UserCurrencyFollows;
using Client.Models.UserCurrencyFollows.RequestParameters;
using Client.Services.UserCurrencyFollows;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class UserCurrencyFollowsController : BaseController
    {
        private readonly IUserCurrencyFollowService _userCurrencyFollowService;

        public UserCurrencyFollowsController(IUserCurrencyFollowService userCurrencyFollowService)
        {
            _userCurrencyFollowService = userCurrencyFollowService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] UserCurrencyFollowRequestParameter parameter)
        {
            var result = await _userCurrencyFollowService.ListAsync(parameter);

            _ = this.ShowResultMessage(result);

            return View(result.Data ?? new());
        }

        [HttpPost]
        public async Task<IActionResult> AddToFav([FromForm] int currencyId, [FromForm] string? redirectUrl)
        {
            var result = await _userCurrencyFollowService.AddAsync(new CurrencyFollowInput(currencyId, false));
            _ = this.ShowResultMessage(result);

            if (string.IsNullOrEmpty(redirectUrl))
                return RedirectToAction(nameof(CurrenciesController.Index), "Currencies");

            return Redirect(redirectUrl);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBroadcast([FromForm] int currencyId, [FromForm] string? redirectUrl)
        {
            var result = await _userCurrencyFollowService.AddAsync(new CurrencyFollowInput(currencyId, true));
            _ = this.ShowResultMessage(result);

            if (string.IsNullOrEmpty(redirectUrl))
                return RedirectToAction(nameof(CurrenciesController.Index), "Currencies");

            return Redirect(redirectUrl);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFav([FromForm] int currencyId, [FromForm] string? redirectUrl)
        {
            var result = await _userCurrencyFollowService.DeleteAsync(currencyId);
            _ = this.ShowResultMessage(result);

            if (string.IsNullOrEmpty(redirectUrl))
                return RedirectToAction(nameof(CurrenciesController.Index), "Currencies");

            return Redirect(redirectUrl);
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateOperation(int userCurrencyFollowId)
        {
            var data = await _userCurrencyFollowService.InfoAsync(userCurrencyFollowId);

            _ = this.ShowResultMessage(data);

            return PartialView("_UserCurrencyFollowPopup", data.Data ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOperation([FromQuery] int userCurrencyFollowId, [FromForm] ChangeCurrencyFollowInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _userCurrencyFollowService.ChangeStatusAsync(userCurrencyFollowId, input);

                _ = this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(UserCurrencyFollowsController.Index), "Assets");
        }
    }
}