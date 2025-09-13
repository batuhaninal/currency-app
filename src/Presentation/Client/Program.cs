using Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<BreadcrumbActionFilter>();
});

builder.Services.BindClientService();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/auth/login";
        // opt.AccessDeniedPath = "";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Index?statusCode=500");
    app.UseStatusCodePagesWithReExecute("/Errors/Index", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Errors/Index?statusCode=500");
app.UseStatusCodePagesWithReExecute("/Errors/Index", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "panel",
        areaName: "panel",
        pattern: "panel/{controller}/{action}/{id?}"
    );

    endpoints.MapDefaultControllerRoute();
});

app.Run();
