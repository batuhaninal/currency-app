using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.UserCurrencyFollows.Add
{
    public sealed record AddUserCurrencyFollowCommand : ICommand
    {
        public AddUserCurrencyFollowCommand()
        {

        }

        public AddUserCurrencyFollowCommand(int currencyId, bool isBroadcast)
        {
            CurrencyId = currencyId;
            IsBroadcast = isBroadcast;
        }

        public int CurrencyId { get; init; }
        public bool IsBroadcast { get; init; }
    }

    public class AddUserCurrencyFollowCommandHandler : ICommandHandler<AddUserCurrencyFollowCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public AddUserCurrencyFollowCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(AddUserCurrencyFollowCommand command, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            int userId = _userTokenService.UserId;

            IBaseResult result = await _unitOfWork.UserRule.CheckExistAsync(userId, cancellationToken);

            if (!result.Success)
                return result;

            result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);

            if (!result.Success)
                return result;

            DateTime now = DateTime.UtcNow;

            await _unitOfWork
                .UserCurrencyFollowWriteRepository
                .CreateAsync(new Domain.Entities.UserCurrencyFollow()
                {
                    CurrencyId = command.CurrencyId,
                    UserId = userId,
                    IsActive = command.IsBroadcast,
                    CreatedDate = now,
                    UpdatedDate = now
                }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(201, true);
        }
    }
} 