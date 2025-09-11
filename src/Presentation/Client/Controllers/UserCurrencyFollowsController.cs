using Client.Models.UserCurrencyFollows;
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
    }
}