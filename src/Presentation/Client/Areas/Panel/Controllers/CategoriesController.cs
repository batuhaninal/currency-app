using System.Text.Json;
using Client.Controllers;
using Client.Models.Categories;
using Client.Models.Categories.RequestParameters;
using Client.Models.Constants;
using Client.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Panel.Controllers
{
    [Area(AppConstants.PANELAREA)]
    [Authorize(Roles = AppConstants.ADMINROLE)]
    public sealed class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CategoryBaseRequestParameter parameter)
        {
            var data = await _categoryService.ListAsync(parameter);
            return View(data.Data ?? new());
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateOperation(int categoryId)
        {
            var data = await _categoryService.InfoAsync(categoryId);

            return PartialView("_CategoryUpdatePopup", data.Data ?? new());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOperation([FromQuery] int categoryId, [FromForm] CategoryInput categoryInput)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _categoryService.UpdateAsync(categoryId, categoryInput);

                if (!result.Success)
                    this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(CategoriesController.Index), "Categories", new { Area = AppConstants.PANELAREA });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOperation([FromForm] CategoryInput categoryInput)
        {
            if (CheckModelStateValid(ModelState))
            {
                var result = await _categoryService.AddAsync(categoryInput);
                if (!result.Success)
                    this.ShowResultMessage(result);
            }

            return RedirectToAction(nameof(CategoriesController.Index), "Categories", new { Area = AppConstants.PANELAREA });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatusOperation([FromQuery] int categoryId)
        {
            var result = await _categoryService.ChangeStatusAsync(categoryId);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteOperation([FromQuery] int categoryId)
        {
            var result = await _categoryService.DeleteAsync(categoryId);
            return Json(result);
        }
    }
}