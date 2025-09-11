using Application;
using Adapter;
using Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using API.Handlers;
using Application.Models.Constants.Roles;
using System.Net;
using Application.Models.DTOs.Commons.Results;
using API.Middlewares;
using API.Middlewares.ExceptionHandlers;
using API.Notifiers;
using API.Hubs;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.BindApplicationServices(builder.Configuration);
builder.Services.BindPersistenceServices(builder.Configuration, builder.Environment);
builder.Services.BindAdapterServices(builder.Configuration);
builder.Services.BindHandlerService();
builder.Services.BindNotifiers(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // sub -> nameidentifier maplenmesini kapattik
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]!)),
        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Name,
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7092") // client port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // signalR için önemli
    });
});


builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessExceptionHandler>();
builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
builder.Services.AddExceptionHandler<ExceptionMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppRoles.Admin, policy => 
        policy
            .RequireRole(AppRoles.Admin));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1");
    });

    app.UseReDoc(opt =>
    {
        opt.SpecUrl("/openapi/v1.json");
    });
}

app.UseExceptionHandler();

app.UseStatusCodePages(async context => // 401 / 403 gibi durum kodlarına özel yanıt
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(new ResultDto((int)HttpStatusCode.Forbidden, false, null, "Access is forbidden."));
    }

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(new ResultDto((int)HttpStatusCode.Unauthorized, false, null, "Unauthorized access."));
    }
});

app.UseHttpsRedirection();
app.RouteMap();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapHubs();

app.Run();