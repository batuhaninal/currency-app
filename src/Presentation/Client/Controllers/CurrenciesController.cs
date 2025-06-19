using Client.Services.Currencies;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class CurrenciesController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
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