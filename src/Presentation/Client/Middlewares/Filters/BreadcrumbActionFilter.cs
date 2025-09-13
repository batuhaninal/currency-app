using Client.Attributes;
using Client.Models.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

public sealed class BreadcrumbActionFilter : IActionFilter
{
    private const string CookieName = "BreadcrumbHistory";
    private const int MaxBreadcrumbs = 5;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller controller) return;

        var httpContext = context.HttpContext;

        // Cookie'den oku
        var history = httpContext.Request.Cookies.GetBreadcrumbs(CookieName);

        var url = httpContext.Request.Path + httpContext.Request.QueryString;

        // 🔹 Attribute'dan title oku
        string title = "Sayfa"; // default
        var cad = context.ActionDescriptor as ControllerActionDescriptor;
        var methodInfo = cad?.MethodInfo;

        if (methodInfo != null)
        {
            var actionAttr = methodInfo.GetCustomAttribute<BreadcrumbAttribute>();
            if (actionAttr != null)
            {
                title = actionAttr.Title;
            }
            else
            {
                var controllerAttr = methodInfo.DeclaringType?.GetCustomAttribute<BreadcrumbAttribute>();
                if (controllerAttr != null)
                    title = controllerAttr.Title;
                else
                {
                    controller.ViewData["BreadcrumbHistory"] = history;
                    return;
                }
            }
        }


        // Boş title ekleme
        if (string.IsNullOrWhiteSpace(title)) title = "Sayfa";

        // Unique: aynı sayfa varsa önce çıkar
        history = history.Where(x => x.Url != url).ToList();

        // Yeni sayfayı ekle
        history.Add(new BreadcrumbItem { Title = title, Url = url });

        // Max 5 adım
        if (history.Count > MaxBreadcrumbs)
            history = history.Skip(history.Count - MaxBreadcrumbs).ToList();

        // Cookie'ye yaz
        httpContext.Response.Cookies.SetObject(CookieName, history);

        // ViewData'ya gönder
        controller.ViewData["BreadcrumbHistory"] = history;
        controller.ViewBag.Title = title;
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
