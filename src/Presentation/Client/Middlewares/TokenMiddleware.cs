
using System.Net;
using System.Net.Http.Headers;
using Client.Models.Constants;
using Client.Services.Auth;

namespace Client.Middlewares
{
    public class TokenMiddleware : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthService _authService;
        private readonly ILogger<TokenMiddleware> _logger;

        public TokenMiddleware(IHttpContextAccessor contextAccessor, IAuthService authService, ILogger<TokenMiddleware> logger)
        {
            _contextAccessor = contextAccessor;
            _authService = authService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = _contextAccessor.HttpContext;

            var accessToken = context.User?.FindFirst("access-token")?.Value;

            if (!string.IsNullOrWhiteSpace(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = context.Request.Cookies[AppConstants.REFRESHTOKEN];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var refreshResult = await _authService.RefreshTokenAsync(context, refreshToken, cancellationToken);
                    if (refreshResult.Success && refreshResult.Data != null)
                    {
                        var newAccessToken = refreshResult.Data.AccessToken;
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();


            return response;
        }
    }
}