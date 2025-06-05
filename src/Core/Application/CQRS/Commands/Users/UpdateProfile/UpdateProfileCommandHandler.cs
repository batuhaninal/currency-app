using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Security;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Users;

namespace Application.CQRS.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;
        private readonly IHashingService _hashingService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService, IHashingService hashingService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
            _hashingService = hashingService;
        }

        public async Task<IBaseResult> Handle(UpdateProfileCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserReadRepository.GetByIdAsync(_userTokenService.UserId, true, cancellationToken);
            if (user == null)
                return new ResultDto(400, false, null, ErrorMessage.USEREXIST);
            if (!string.IsNullOrEmpty(command.OldPassword) && !string.IsNullOrEmpty(command.NewPassword))
            {
                bool pswValid = _hashingService.VerifyPassword(command.OldPassword, user.PasswordHash, user.PasswordSalt);

                if (!pswValid)
                    return new ResultDto(400, false, null, ErrorMessage.USERPASSWORDINVALID);

                byte[] pswHash, pswSalt;

                _hashingService.CreatePasswordHash(command.NewPassword, out pswHash, out pswSalt);

                command.ToDomain(ref user, pswHash, pswSalt);
            }
            else
            {
                command.ToDomain(ref user);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(203, true, new UserRelationDto(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName
            ));
        }
    }
}