using Client.Attributes;
using Client.Models.Commons;
using Client.Models.Currencies.RequestParameters;
using Client.Models.UserCurrencyFollows.RequestParameters;
using Client.Services.Currencies;
using Client.Services.Tools;
using Client.Services.UserCurrencyFollows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Authorize]
    [Breadcrumb("Birimler")]
    public sealed class CurrenciesController : BaseController
    {
        private readonly ICurrencyService _currencyService;
        private readonly IToolService _toolService;
        private readonly IUserCurrencyFollowService _userCurrencyFollowService;


        public CurrenciesController(ICurrencyService currencyService, IToolService toolService, IUserCurrencyFollowService userCurrencyFollowService)
        {
            _currencyService = currencyService;
            _toolService = toolService;
            _userCurrencyFollowService = userCurrencyFollowService;
        }

        [HttpGet]
        [Breadcrumb("Birimler Listesi")]
        public async Task<IActionResult> Index([FromQuery] CurrencyRequestParameter parameters)
        {
            var result = await _toolService.CategoryTools(new ToolRequestParameter
            {
                IsActive = true,
                OrderBy = "title"
            });
            _ = this.ShowResultMessage(result);

            ViewBag.CategoryTool = result.Data ?? new();

            var follows = await _userCurrencyFollowService.TopFollowList(new BroadcastParameters() { All = true, IsBroadcast = false });

            ViewBag.Follows = follows.Data ?? new int[] { };

            var data = await _currencyService.EUListAsync(parameters);
            _ = this.ShowResultMessage(data);

            return View(data.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> CalculatorPopup()
        {
            var result = await _currencyService.CalculatorAsync();
            _ = this.ShowResultMessage(result);

            return PartialView("_CalculatorPopup", result.Data ?? new());
        }
    }
}