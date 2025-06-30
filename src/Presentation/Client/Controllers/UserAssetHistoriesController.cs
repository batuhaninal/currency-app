using Client.Models.UserAssetHistories.RequestParameters;
using Client.Services.UserAssetHistories;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public sealed class UserAssetHistoriesController : BaseController
    {
        private readonly IUserAssetHistoryService _userAssetHistoryService;

        public UserAssetHistoriesController(IUserAssetHistoryService userAssetHistoryService)
        {
            _userAssetHistoryService = userAssetHistoryService;
        }

        [HttpGet]
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
            return PartialView("_UserAssetItemHistoryPopup",result.Data ?? new());
        }
    }
}