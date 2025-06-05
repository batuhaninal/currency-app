using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Security;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Users.Login
{
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IHashingService hashingService, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
            _tokenService = tokenService;
        }

        public async Task<IBaseResult> Handle(LoginCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.UserRule.CheckExistAsync(command.Username);

            if (!result.Success)
                return result;

            User user = (await _unitOfWork.UserReadRepository.Table.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Email == command.Username))!;
            bool pswResult = _hashingService.VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt);

            if (!pswResult)
                return new ResultDto(400, false, null, ErrorMessage.USEREXIST);

            var token = _tokenService.CreateAccessToken(user, 60 * 24 * 30);
            return new ResultDto(200, pswResult, token);
        }
    }
}