
using System.Diagnostics;
using Application.Abstractions.Commons.Logger;

namespace Application.CQRS.Commons.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    {
        private readonly ILoggerService<PerformanceBehaviour<TRequest, TResponse>> _logger;

        public PerformanceBehaviour(ILoggerService<PerformanceBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            _logger.Info(string.Format("Request {0} executed in {1}ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds));

            return response;
        }
    }
}