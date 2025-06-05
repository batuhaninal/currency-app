using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Client.Models.Auth;
using Client.Models.Commons;
using Client.Models.Constants;
using Client.Services.Commons;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Client.Services.Auth
{
    public sealed class AuthService : BaseService, IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<LoginResponse>> LoginAsync(HttpContext httpContext, LoginInput input, CancellationToken cancellationToken = default)
        {
            var data = await this.PostAsync<LoginInput, LoginResponse>(_httpClient, "login", input, cancellationToken);
            if (!data.Success)
                return data;

            await LoginToAppAsync(httpContext, data.Data);

            return data;
        }

        public async Task<BaseResult<LoginResponse>> RegisterAsync(HttpContext httpContext, RegisterInput input, CancellationToken cancellationToken = default)
        {
            var data = await this.PostAsync<RegisterInput, LoginResponse>(_httpClient, "register", input, cancellationToken);
            if (!data.Success)
                return data;

            await LoginToAppAsync(httpContext, data.Data);

            return data;
        }

        public async Task<BaseResult<NoContent>> LogoutAsync(HttpContext httpContext)
        {
            try
            {
                httpContext.Response.Cookies.Delete("refreshToken");

                await httpContext.SignOutAsync();

                return new BaseResult<NoContent>(203, true);
            }
            catch (System.Exception)
            {
                return new BaseResult<NoContent>(500, false, null, "Beklenmeyen Hata!");
            }
        }

        public async Task<BaseResult<LoginResponse>> RefreshTokenAsync(HttpContext httpContext, string refreshToken, CancellationToken cancellationToken = default)
        {
            var data = await this.PostAsync<RefreshTokenInput, LoginResponse>(_httpClient, "refresh-token", new RefreshTokenInput(refreshToken), cancellationToken);

            if (!data.Success)
                return data;

            await LoginToAppAsync(httpContext, data.Data);

            return data;
        }

        private async Task LoginToAppAsync(HttpContext httpContext, LoginResponse? response)
        {
            if (response == null || string.IsNullOrWhiteSpace(response.AccessToken))
                throw new UnauthorizedAccessException("Access token bulunamadı.");
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(response.AccessToken);

                var claims = jwtToken.Claims.ToList();
                claims.Add(new Claim("access-token", response.AccessToken));

                httpContext.Response.Cookies.Append(AppConstants.REFRESHTOKEN, response.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = response.Expiration
                });

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = response.Expiration.AddMinutes(-3),
                });
            }
            catch (System.Exception)
            {
                throw new UnauthorizedAccessException();
            }

        }
    }
}