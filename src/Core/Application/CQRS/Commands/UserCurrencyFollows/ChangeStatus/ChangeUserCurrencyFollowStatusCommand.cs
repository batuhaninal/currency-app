using System.Text.Json.Serialization;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.UserCurrencyFollows.ChangeStatus
{
    public sealed record ChangeUserCurrencyFollowStatusCommand : ICommand
    {
        public ChangeUserCurrencyFollowStatusCommand()
        {

        }

        public ChangeUserCurrencyFollowStatusCommand(int userCurrencyFollowId, bool isActive)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
            IsActive = isActive;
        }

        [JsonIgnore]
        public int UserCurrencyFollowId { get; set; }
        public bool IsActive { get; init; }
    }

    public sealed class ChangeUserCurrencyFollowStatusCommandHandler : ICommandHandler<ChangeUserCurrencyFollowStatusCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public ChangeUserCurrencyFollowStatusCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(ChangeUserCurrencyFollowStatusCommand command, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            int userId = _userTokenService.UserId;

            IBaseResult result = await _unitOfWork
                .UserCurrencyFollowRule
                .CheckExistAsync(command.UserCurrencyFollowId, userId, cancellationToken);

            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .FirstOrDefaultAsync(x => x.Id == command.UserCurrencyFollowId && x.UserId == userId, cancellationToken);

            if (data is not null)
            {
                data.IsActive = command.IsActive;

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new ResultDto(203, true);
        }
    }
}