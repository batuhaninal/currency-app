using System.Net;
using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.Commons.Results;
using Application.Utilities.Exceptions.Commons;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middlewares.ExceptionHandlers
{
    public sealed class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<NotFoundExceptionHandler> _logger;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException notFoundException)
                return false;

            _logger.LogError("{ServiceName} service exception : {Error} at : {Now}", typeof(NotFoundExceptionHandler).Name, notFoundException.Message, DateTime.UtcNow);

            IBaseResult result = new ResultDto((int)HttpStatusCode.NotFound, false, null, notFoundException.Message);

            httpContext.Response.StatusCode = result.StatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext
                .Response
                .WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}