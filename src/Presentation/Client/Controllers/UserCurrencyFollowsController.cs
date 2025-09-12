using Client.Models.Commons;
using Client.Models.UserCurrencyFollows;
using Client.Models.UserCurrencyFollows.RequestParameters;
using Client.Services.Currencies;
using Client.Services.Tools;
using Client.Services.UserCurrencyFollows;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class UserCurrencyFollowsController : BaseController
    {
        private readonly IUserCurrencyFollowService _userCurrencyFollowService;
        private readonly IToolService _toolService;

        public UserCurrencyFollowsController(IUserCurrencyFollowService userCurrencyFollowService, IToolService toolService)
        {
            _userCurrencyFollowService = userCurrencyFollowService;
            _toolService = toolService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] UserCurrencyFollowRequestParameter parameter)
        {
            var result = await _userCurrencyFollowService.ListAsync(parameter);

            _ = this.ShowResultMessage(result);

            var follows = await _userCurrencyFollowService.TopFollowList(new BroadcastParameters() { All = false, IsBroadcast = true });

            _ = this.ShowResultMessage(follows);

            ViewBag.Follows = follows.Data ?? new int[] { };

            var currencyTools = await _toolService.CurrencyTools(new ToolRequestParameter() { });

            _ = this.ShowResultMessage(currencyTools);

            ViewBag.CurrencyTool = currencyTools.Data ?? new();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOperation([FromQuery] int userCurrencyFollowId, [FromForm] ChangeCurrencyFollowInput input, [FromForm] string? redirectUrl)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _userCurrencyFollowService.ChangeStatusAsync(userCurrencyFollowId, input);

                _ = this.ShowResultMessage(result);
            }

            if (string.IsNullOrEmpty(redirectUrl))
                return RedirectToAction(nameof(UserCurrencyFollowsController.Index), "UserCurrencyFollows");

            return Redirect(redirectUrl);
        }
    }
}