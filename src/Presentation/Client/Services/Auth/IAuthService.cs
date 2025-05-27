using Client.Models.Auth;
using Client.Models.Commons;

namespace Client.Services.Auth
{
    public interface IAuthService
    {
        Task<BaseResult<LoginResponse>> LoginAsync(HttpContext httpContext, LoginInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<LoginResponse>> RegisterAsync(HttpContext httpContext, RegisterInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> LogoutAsync(HttpContext httpContext);
        Task<BaseResult<LoginResponse>> RefreshTokenAsync(HttpContext httpContext, string refreshToken, CancellationToken cancellationToken = default);
    }
}