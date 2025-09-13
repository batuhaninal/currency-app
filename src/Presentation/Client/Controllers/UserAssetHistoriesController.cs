using Client.Attributes;
using Client.Models.UserAssetHistories.RequestParameters;
using Client.Services.UserAssetHistories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Authorize]
    [Breadcrumb("Varlik Gecmisi")]
    public sealed class UserAssetHistoriesController : BaseController
    {
        private readonly IUserAssetHistoryService _userAssetHistoryService;

        public UserAssetHistoriesController(IUserAssetHistoryService userAssetHistoryService)
        {
            _userAssetHistoryService = userAssetHistoryService;
        }

        [HttpGet]
        [Breadcrumb("Varlik Gecmisi Listesi")]
        public async Task<IActionResult> Index([FromQuery] UserAssetHistoryRequestParameter parameter)
        {
            var result = await _userAssetHistoryService.ListAsync(parameter);
            _ = this.ShowResultMessage(result);
            return View(result.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> UserAssetItemHistoryPopup([FromQuery] int userAssetHistoryId)
        {
            var result = await _userAssetHistoryService.ItemListAsync(userAssetHistoryId);
            _ = this.ShowResultMessage(result);
            return PartialView("_UserAssetItemHistoryPopup", result.Data ?? new());
        }

        [HttpPost]
        public async Task<IActionResult> Save()
        {
            var result = await _userAssetHistoryService.SaveAsync();
            _ = this.ShowResultMessage(result);
            return RedirectToAction(nameof(UserAssetHistoriesController.Index), "UserAssetHistories");
        }
    }
}