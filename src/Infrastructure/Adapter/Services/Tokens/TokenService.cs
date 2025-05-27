using Application.Abstractions.Commons.Tokens;
using Application.Models.Constants.Roles;
using Application.Models.Tokens;
using Application.Utilities.Exceptions.Commons;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Adapter.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtToken CreateAccessToken(User user, int minutes)
        {
            if (user.UserRoles == null ||
                !user.UserRoles.Any() ||
                user.UserRoles.Select(x=> x.Role).FirstOrDefault() == null ||
                !user.UserRoles.Select(x=> x.Role).Any())
                throw new BusinessException("Invalid Token with User");

            JwtToken token = new();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration.GetSection("Token")["SecurityKey"]!));

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddMinutes(minutes);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("sub", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRoles!.FirstOrDefault()!.Role!.Name)
            };

            JwtSecurityToken securityToken = new JwtSecurityToken(
                audience: _configuration.GetSection("Token")["Audience"],
                issuer: _configuration.GetSection("Token")["Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims);

            JwtSecurityTokenHandler tokenHandler = new();

            token.AccessToken = tokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
