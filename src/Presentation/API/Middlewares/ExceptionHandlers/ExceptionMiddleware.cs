using System.Net;
using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.Commons.Results;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middlewares
{
    public sealed class ExceptionMiddleware : IExceptionHandler
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("{ServiceName} service exception : {Error} at : {Now}", typeof(ExceptionMiddleware).Name, exception.Message, DateTime.UtcNow);

            IBaseResult result = new ResultDto((int)HttpStatusCode.InternalServerError, false, null, "An unexpected error occurred.");

            httpContext.Response.StatusCode = result.StatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response
                .WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}