using Client.Controllers;
using Client.Models.Commons;
using Client.Models.Constants;
using Client.Models.Currencies;
using Client.Models.Currencies.RequestParameters;
using Client.Services.Currencies;
using Client.Services.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Panel.Controllers
{
    [Area(AppConstants.PANELAREA)]
    [Authorize(Roles = AppConstants.ADMINROLE)]
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
        public async Task<IActionResult> Index([FromQuery] CurrencyRequestParameter parameter)
        {
            var result = await _toolService.CategoryTools(new ToolRequestParameter());
            if (!result.Success)
                TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";

            ViewBag.CategoryTool = result.Data ?? new();

            var data = await _currencyService.ListAsync(parameter);
            return View(data.Data ?? new());
        }

        [HttpGet]
        public async Task<IActionResult> Info([FromQuery] int currencyId, [FromQuery] CurrencyHistoryRequestParameter parameter)
        {
            var result = await _currencyService.HistoryInfoAsync(currencyId, parameter);
            _ = this.ShowResultMessage(result);

            return View(result.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateOperation(int currencyId)
        {
            var data = await _currencyService.InfoAsync(currencyId);

            var result = await _toolService.CategoryTools(new ToolRequestParameter());
            if (!result.Success)
                TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";

            ViewBag.CategoryTool = result.Data ?? new();

            return PartialView("_CurrencyUpdatePopup", data.Data ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOperation([FromQuery] int currencyId, [FromForm] CurrencyInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _currencyService.UpdateAsync(currencyId, input);

                if (!result.Success)
                    TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";
            }

            return RedirectToAction(nameof(CurrenciesController.Index), "Currencies", new { Area = AppConstants.PANELAREA });
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdatePriceOperation(int currencyId)
        {
            var data = await _currencyService.PriceInfoAsync(currencyId);

            return PartialView("_CurrencyPriceUpdatePopup", data.Data ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePriceOperation([FromQuery] int currencyId, [FromForm] CurrencyPriceInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _currencyService.UpdatePriceAsync(currencyId, input);

                if (!result.Success)
                    TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";
            }

            return RedirectToAction(nameof(CurrenciesController.Index), "Currencies", new { Area = AppConstants.PANELAREA });
        }

        [HttpGet]
        public async Task<IActionResult> AddOperation()
        {
            var result = await _toolService.CategoryTools(new ToolRequestParameter());
            if (!result.Success)
                TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";

            ViewBag.CategoryTool = result.Data ?? new();

            return PartialView("_CurrencyAddPopup", new CurrencyInput());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOperation([FromForm] CurrencyInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _currencyService.AddAsync(input);
                if (!result.Success)
                    TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";
            }

            return RedirectToAction(nameof(CurrenciesController.Index), "Currencies", new { Area = AppConstants.PANELAREA });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatusOperation([FromQuery] int currencyId)
        {
            var result = await _currencyService.ChangeStatusAsync(currencyId);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteOperation([FromQuery] int currencyId)
        {
            var result = await _currencyService.DeleteAsync(currencyId);
            return Json(result);
        }
    }
}