using System.Threading.Tasks;
using Client.Controllers;
using Client.Models.Constants;
using Client.Models.Currencies.RequestParameters;
using Client.Services.Currencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Panel.Controllers
{
    [Area(AppConstants.PANELAREA)]
    [Authorize(Roles = AppConstants.ADMINROLE)]
    public sealed class CurrenciesController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CurrencyRequestParameter parameter)
        {
            var data = await _currencyService.ListAsync(parameter);
            return View(data.Data ?? new());
        }
    }
}