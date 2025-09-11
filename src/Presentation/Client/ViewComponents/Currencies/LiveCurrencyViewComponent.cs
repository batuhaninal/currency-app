using Client.Models.Currencies;
using Client.Services.Currencies;
using Microsoft.AspNetCore.Mvc;

namespace Client.ViewComponents.Currencies
{
    public class LiveCurrencyViewComponent : ViewComponent
    {
        private readonly ICurrencyService _currencyService;

        public LiveCurrencyViewComponent(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int currencyId)
        {
            var result = await _currencyService.EUInfoAsync(currencyId);
            return View(result.Data ?? new());
        }
    }
}