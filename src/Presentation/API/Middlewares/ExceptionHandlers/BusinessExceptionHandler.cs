using System.Net;
using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.Commons.Results;
using Application.Utilities.Exceptions.Commons;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middlewares.ExceptionHandlers
{
    public sealed class BusinessExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BusinessExceptionHandler> _logger;

        public BusinessExceptionHandler(ILogger<BusinessExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BusinessException businessEx)
                return false;

            _logger.LogError("{ServiceName} service exception : {Error} at : {Now}", typeof(BusinessExceptionHandler).Name, businessEx.Message, DateTime.UtcNow);

            IBaseResult result = new ResultDto((int)HttpStatusCode.BadRequest, false, null, businessEx.Message);

            httpContext.Response.StatusCode = result.StatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext
                .Response
                .WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}