using Application.Abstractions.Commons.Tokens;
using Application.Models.Constants.Roles;
using Application.Utilities.Exceptions.Commons;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Adapter.Services.Tokens
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserTokenService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool IsAuthenticated => _contextAccessor!.HttpContext!.User!.Identity!.IsAuthenticated;

        public string UserEmail => CheckRules(ClaimTypes.Email);

        public int UserId => int.Parse(CheckRules("sub"));

        public bool IsAdmin => CheckRules(ClaimTypes.Role).Equals(AppRoles.Admin);
        private string CheckRules(string claimType)
        {
            if (!IsAuthenticated)
                throw new BusinessException("Token Exception! Please Re-Authenticated");

            string? value = _contextAccessor.HttpContext?.User.FindFirst(claimType)?.Value;

            // sub -> nameidentifier auto map devrede ise
            if (claimType == "sub" && string.IsNullOrEmpty(value))
                value = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(value))
                throw new BusinessException("Token Exception! Please Re-Authenticated");

            return value;
        }
    }
}
