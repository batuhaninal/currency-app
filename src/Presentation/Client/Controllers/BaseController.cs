using System.Text.Json;
using Client.Models.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Client.Controllers
{
    public abstract class BaseController : Controller
    {
        public virtual bool CheckModelStateValid(ModelStateDictionary modelState)
        {
            bool state = modelState.IsValid;
            if (!state)
            {
                var errorMessages = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                TempData["ValidationErrors"] = JsonSerializer.Serialize(errorMessages);
            }

            return state;
        }

        public virtual bool ShowResultMessage<T>(BaseResult<T> result)
        {
            bool valid = result.Success;
            if (valid)
            {
                TempData["Success"] = result.Message ?? "Başarılı";
            }
            else
            {
                string errorMessage = result.Message ?? "Beklenmeyen Hata!";
                string validationMessage = "";

                if (result.ValidationErrors != null && result.ValidationErrors.Any())
                {
                    validationMessage = JsonSerializer.Serialize(result.ValidationErrors);
                }

                TempData["Errors"] = errorMessage;
                TempData["DbValidationErrors"] = validationMessage;
            }

            return valid;
        }
    }
}