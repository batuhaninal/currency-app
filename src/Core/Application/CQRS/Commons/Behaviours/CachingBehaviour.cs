using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Logger;
using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commons.Behaviours
{
    public class CachingBehaviour<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : IQuery
    {
        private readonly ICacheService _cacheService;
        private readonly ILoggerService<CachingBehaviour<TRequest, TResponse>> _logger;

        public CachingBehaviour(ICacheService cacheService, ILoggerService<CachingBehaviour<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            if (request is not ICacheableQuery cacheable)
                return await next();

            var cacheKey = cacheable.CacheKey;
            _logger.Info(string.Format("Checking cache for key: {0}", cacheKey));

            var cacheData = await _cacheService.GetAsync(cacheKey);

            // if (!string.IsNullOrEmpty(cacheData))
            // {
            //     _logger.Info(string.Format("Cache hit for key: {CacheKey}", cacheKey));
            //     // Tip guvenligi sıkıntı!
            //     object? data = JsonSerializer.Deserialize<object>(cacheData);

            //     return new ResultDto(200, true, data)(IBaseResult);
            // }

            throw new NotImplementedException();
        }
    }
}