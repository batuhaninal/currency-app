using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.UserRoles;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class UserRoleRule : IUserRoleRule
    {
        private readonly IUserRoleReadRepository _userRoleReadRepository;

        public UserRoleRule(IUserRoleReadRepository userRoleReadRepository)
        {
            _userRoleReadRepository = userRoleReadRepository;
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _userRoleReadRepository.AnyAsync(x=> x.Id == id, cancellationToken);
            return new ResultDto(400, result, null, ErrorMessage.USERROLEEXIST);
        }

        public async Task<IBaseResult> CheckUserRoleValidForUserAsync(int userId, int roleId, CancellationToken cancellationToken = default)
        {
            bool result = await _userRoleReadRepository.AnyAsync(x=> x.UserId == userId && x.RoleId == roleId, cancellationToken);
            return new ResultDto(400, !result, null, ErrorMessage.ROLEDUPLICATEFORUSER);
        }
    }
}