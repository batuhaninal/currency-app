using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Security;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.CQRS.Queries.Users.GetProfile;
using Application.Models.Constants.CachePrefixes;
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
        private readonly ICacheService _cacheService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService, IHashingService hashingService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
            _hashingService = hashingService;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(UpdateProfileCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserReadRepository.GetByIdAsync(_userTokenService.UserId, true, cancellationToken);
            if (user == null)
                return new ResultDto(400, false, null, ErrorMessage.USEREXIST);

            bool willCache = false;

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
                willCache = true;
                command.ToDomain(ref user);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            string cacheKey = CachePrefix.CreateByParameter(CachePrefix.UserPrefix, nameof(GetProfileQueryHandler), "email", _userTokenService.UserEmail);
            
            if(willCache)
                await _cacheService.DeleteAsync(cacheKey);

            return new ResultDto(200, true, new UserRelationDto(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName
            ));
        }
    }
}