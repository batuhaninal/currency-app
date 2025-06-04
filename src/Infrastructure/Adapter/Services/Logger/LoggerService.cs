using Application.Abstractions.Commons.Logger;
using Microsoft.Extensions.Logging;

namespace Adapter.Services.Logger
{
    public sealed class LoggerService<TClass> : ILoggerService<TClass>
        where TClass : class
    {
        private readonly ILogger<TClass> _logger;

        public LoggerService(ILogger<TClass> logger)
        {
            _logger = logger;
        }

        public void Error(string message) => _logger.LogError("{Now} error occured on : {ServiceName} error : {Error}", DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"), typeof(TClass).Name, message);

        public void Info(string message) => _logger.LogInformation("{Now} on {ServiceName} info : {Message}", DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"), typeof(TClass).Name, message);
    }
}