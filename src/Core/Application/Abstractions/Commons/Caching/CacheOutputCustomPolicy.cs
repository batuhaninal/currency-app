using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Application.Abstractions.Commons.Caching
{
    public sealed class CacheOutputCustomPolicy : IOutputCachePolicy
    {
        public static readonly CacheOutputCustomPolicy Instance = new CacheOutputCustomPolicy();

        public CacheOutputCustomPolicy()
        {
            
        }

        ValueTask IOutputCachePolicy.CacheRequestAsync(
        OutputCacheContext context,
        CancellationToken cancellationToken)
        {
            var attemptOutputCaching = AttemptOutputCaching(context);
            context.EnableOutputCaching = true;
            context.AllowCacheLookup = attemptOutputCaching;
            context.AllowCacheStorage = attemptOutputCaching;
            context.AllowLocking = true;

            // Vary by any query by default
            context.CacheVaryByRules.QueryKeys = "*";

            return ValueTask.CompletedTask;
        }

        ValueTask IOutputCachePolicy.ServeFromCacheAsync
            (OutputCacheContext context, CancellationToken cancellationToken)
        {
            return ValueTask.CompletedTask;
        }

        ValueTask IOutputCachePolicy.ServeResponseAsync
            (OutputCacheContext context, CancellationToken cancellationToken)
        {
            var response = context.HttpContext.Response;

            // Verify existence of cookie headers
            if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            // Check response code
            if (response.StatusCode != StatusCodes.Status200OK &&
                response.StatusCode != StatusCodes.Status301MovedPermanently)
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            return ValueTask.CompletedTask;
        }

        private static bool AttemptOutputCaching(OutputCacheContext context)
        {
            // Check if the current request fulfills the requirements
            // to be cached
            var request = context.HttpContext.Request;

            // Verify the method
            if (!HttpMethods.IsGet(request.Method) &&
                !HttpMethods.IsHead(request.Method) &&
                !HttpMethods.IsPost(request.Method))
            {
                return false;
            }

            int pageIndex = request.Query["PageIndex"] != StringValues.Empty ? int.Parse(request.Query["PageIndex"].ToString()) : 0;
            int pageSize = request.Query["PageSize"] != StringValues.Empty ? int.Parse(request.Query["PageSize"].ToString()) : 0;
            int queryCount = request.Query.Count;

            if (queryCount == 2 &&
                pageIndex != 0 &&
                pageSize != 0 &&
                pageIndex > 0 &&
                pageIndex <= 5 &&
                (pageSize == 5 ||
                pageSize == 10 ||
                pageSize == 20 ||
                pageSize == 25 ||
                pageSize == 50))
                return true;
            else
                return false;

            // Verify existence of authorization headers
            // Authenticated users can use cache output
            //if (!StringValues.IsNullOrEmpty(request.Headers.Authorization) ||
            //    request.HttpContext.User?.Identity?.IsAuthenticated == true)
            //{
            //    return false;
            //}
        }
    }
}
