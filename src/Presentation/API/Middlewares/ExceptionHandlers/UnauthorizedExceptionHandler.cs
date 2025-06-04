using System.Net;
using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.Commons.Results;
using Application.Utilities.Exceptions.Commons;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middlewares.ExceptionHandlers
{
    public sealed class UnauthorizedExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<UnauthorizedExceptionHandler> _logger;

        public UnauthorizedExceptionHandler(ILogger<UnauthorizedExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not UnauthorizedAccessException unauthEx)
                return false;

            _logger.LogError("{ServiceName} service exception : {Error} at : {Now}", typeof(UnauthorizedExceptionHandler).Name, unauthEx.Message, DateTime.UtcNow);

            IBaseResult result = new ResultDto((int)HttpStatusCode.Unauthorized, false, null, "Please Log-in system");

            httpContext.Response.StatusCode = result.StatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext
                .Response
                .WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}