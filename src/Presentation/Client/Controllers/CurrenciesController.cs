using Client.Models.Commons;
using Client.Models.Currencies.RequestParameters;
using Client.Services.Currencies;
using Client.Services.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class CurrenciesController : BaseController
    {
        private readonly ICurrencyService _currencyService;
        private readonly IToolService _toolService;

        public CurrenciesController(ICurrencyService currencyService, IToolService toolService)
        {
            _currencyService = currencyService;
            _toolService = toolService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CurrencyRequestParameter parameters)
        {
            var result = await _toolService.CategoryTools(new ToolRequestParameter
            {
                IsActive = true,
                OrderBy = "title"
            });
            _ = this.ShowResultMessage(result);

            ViewBag.CategoryTool = result.Data ?? new();

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