using Client.Attributes;
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
        private readonly ICurrencyTagService _currencyTagService;
        private readonly IToolService _toolService;

        public CurrenciesController(ICurrencyService currencyService, ICurrencyTagService currencyTagService, IToolService toolService)
        {
            _currencyService = currencyService;
            _currencyTagService = currencyTagService;
            _toolService = toolService;
        }

        [HttpGet]
        [Breadcrumb("Birimler Listesi")]
        public async Task<IActionResult> Index([FromQuery] CurrencyRequestParameter parameter)
        {
            var result = await _toolService.CategoryTools(new ToolRequestParameter());
            _ = this.ShowResultMessage(result);

            ViewBag.CategoryTool = result.Data ?? new();

            var data = await _currencyService.ListAsync(parameter);
            return View(data.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateOperation(int currencyId)
        {
            var data = await _currencyService.InfoAsync(currencyId);

            var result = await _toolService.CategoryTools(new ToolRequestParameter());
            _ = this.ShowResultMessage(result);

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
                    _ = this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(CurrenciesController.Index), "Currencies", new { Area = AppConstants.PANELAREA });
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateTagOperation(int currencyTagId)
        {
            var data = await _currencyTagService.InfoAsync(currencyTagId);

            var result = await _toolService.CurrencyTools(new());
            _ = this.ShowResultMessage(result);

            ViewBag.CurrencyTool = result.Data ?? new();

            return PartialView("_CurrencyTagUpdatePopup", data.Data ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTagOperation([FromQuery] int currencyTagId, [FromForm] CurrencyTagInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _currencyTagService.UpdateAsync(currencyTagId, input);

                if (!result.Success)
                    _ = this.ShowResultMessage(result);
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
                    _ = this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(CurrenciesController.Index), "Currencies", new { Area = AppConstants.PANELAREA });
        }

        [HttpGet]
        public async Task<IActionResult> AddOperation()
        {
            var result = await _toolService.CategoryTools(new ToolRequestParameter());
            _ = this.ShowResultMessage(result);

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
                    _ = this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(CurrenciesController.Index), "Currencies", new { Area = AppConstants.PANELAREA });
        }

        [HttpGet]
        public async Task<PartialViewResult> AddTagOperation(int currencyId)
        {
            return PartialView("_CurrencyTagAddPopup", new CurrencyTagInput(currencyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTagOperation([FromForm] CurrencyTagInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _currencyTagService.AddAsync(input);
                if (!result.Success)
                    _ = this.ShowResultMessage(result);
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

        [HttpPost]
        public async Task<JsonResult> DeleteTagOperation([FromQuery] int currencyTagId)
        {
            var result = await _currencyTagService.DeleteAsync(currencyTagId);
            return Json(result);
        }
    }
}