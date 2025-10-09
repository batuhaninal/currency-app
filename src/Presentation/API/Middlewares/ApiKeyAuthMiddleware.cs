using Application.Abstractions.Commons.Logger;
using Application.Models.DTOs.Commons.Results;
using Microsoft.AspNetCore.Http.Extensions;

namespace API.Middlewares
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService<ApiKeyAuthMiddleware> _logger;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration, ILoggerService<ApiKeyAuthMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (path != null && path.StartsWith("/hubs/", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }
            
            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new ResultDto(StatusCodes.Status401Unauthorized, false, null, "API Key Missed!"));

                _logger.Error("401: API Key Missed");

                return;
            }

            if (extractedApiKey != _configuration.GetValue<string>("APIKEY"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(new ResultDto(StatusCodes.Status401Unauthorized, false, null, "API Key Missed!"));

                _logger.Error("401: API Key unmatched");
                return;
            }

            await _next(context);
        }
    }
}