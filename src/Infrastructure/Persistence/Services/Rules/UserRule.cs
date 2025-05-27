using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Users;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class UserRule : IUserRule
    {
        private readonly IUserReadRepository _userReadRepository;

        public UserRule(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<IBaseResult> CheckEmailValidAsync(string email, CancellationToken cancellationToken = default)
        {
            bool result = await _userReadRepository.AnyAsync(x=> x.Email == email, cancellationToken);

            return new ResultDto(400, !result, null, ErrorMessage.USEREMAILDUPLICATE);
        }

        public async Task<IBaseResult> CheckEmailValidAsync(int id, string email, CancellationToken cancellationToken = default)
        {
            bool result = await _userReadRepository.AnyAsync(x=> x.Id != id && x.Email == email, cancellationToken);

            return new ResultDto(400, !result, null, ErrorMessage.USEREMAILDUPLICATE);
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _userReadRepository.AnyAsync(x=> x.Id == id, cancellationToken);

            return new ResultDto(400, result, null, ErrorMessage.USEREXIST);
        }

        public async Task<IBaseResult> CheckExistAsync(string email, CancellationToken cancellationToken = default)
        {
            bool result = await _userReadRepository.AnyAsync(x=> x.Email == email, cancellationToken);

            return new ResultDto(400, result, null, ErrorMessage.USEREXIST);
        }
    }
}