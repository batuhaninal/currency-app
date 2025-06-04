using System.Net;
using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.Commons.Results;
using Application.Utilities.Exceptions.Commons;
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

            IBaseResult result;

            switch (exception)
            {
                case BusinessException businessException:
                    result = new ResultDto((int)HttpStatusCode.BadRequest, false, null, businessException.Message);
                    break;

                case UnauthorizedAccessException:
                    result = new ResultDto((int)HttpStatusCode.Unauthorized, false, null, "Unauthorized access.");
                    break;

                default:
                    result = new ResultDto((int)HttpStatusCode.InternalServerError, false, null, "An unexpected error occurred.");
                    break;
            }

            httpContext.Response.StatusCode = result.StatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response
                .WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}