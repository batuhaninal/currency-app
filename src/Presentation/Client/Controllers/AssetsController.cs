using Client.Models.Assets;
using Client.Models.Assets.RequestParameters;
using Client.Models.Commons;
using Client.Services.Assets;
using Client.Services.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Authorize]
    public sealed class AssetsController : BaseController
    {
        private readonly IAssetService _assetService;
        private readonly IToolService _toolService;

        public AssetsController(IAssetService assetService, IToolService toolService)
        {
            _assetService = assetService;
            _toolService = toolService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetRequestParameter parameter)
        {
            var result = await _assetService.UserAssetsAsync(parameter);
            _ = this.ShowResultMessage(result);

            var data = await _toolService.CurrencyTools(new ToolRequestParameter());
            _ = this.ShowResultMessage(data);

            ViewBag.CurrencyTool = data.Data ?? new();

            return View(result.Data ?? new());
        }

        [HttpGet]
        public async Task<IActionResult> UserAssetWithGroup()
        {
            var result = await _assetService.UserAssetsWithGroupAsync();
            _ = this.ShowResultMessage(result);

            return View(result.Data ?? new());
        }

        [HttpGet]
        public async Task<IActionResult> Info([FromQuery] int assetId)
        {
            var result = await _assetService.UserAssetInfoAsync(assetId);
            _ = this.ShowResultMessage(result);

            return View(result.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> UserSummary()
        {
            var result = await _assetService.UserAssetSummaryAsync();
            _ = this.ShowResultMessage(result);

            return PartialView("_AssetSummaryPopup", result.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> AddOperation()
        {
            var result = await _toolService.CurrencyTools(new ToolRequestParameter());
            if (!result.Success)
                TempData["ErrorMessage"] = result.Message ?? "Beklenmeyen hata!";

            ViewBag.CurrencyTool = result.Data ?? new();

            return PartialView("_AssetAddPopup", new AssetAddInput());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOperation([FromForm] AssetAddInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _assetService.AddAsync(input);

                _ = this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(AssetsController.Index), "Assets");
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateOperation(int assetId)
        {
            var data = await _assetService.GetForUpdateAsync(assetId);

            return PartialView("_AssetUpdatePopup", data.Data ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOperation([FromQuery] int assetId, [FromForm] AssetUpdateInput input)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _assetService.UpdateAsync(assetId, input);

                _ = this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(AssetsController.Index), "Assets");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteOperation([FromQuery] int assetId)
        {
            var result = await _assetService.DeleteAsync(assetId);
            return Json(result);
        }
    }
}