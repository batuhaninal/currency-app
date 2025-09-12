using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Domain.Entities;

namespace Application.CQRS.Commands.UserCurrencyFollows.AddRange
{
    public sealed record AddRangeUserCurrencyFollowCommand : ICommand
    {
        public AddRangeUserCurrencyFollowCommand()
        {
            CurrencyIds = new int[] { };
        }

        public AddRangeUserCurrencyFollowCommand(int[] currencyIds, bool isActive)
        {
            CurrencyIds = currencyIds;
            IsActive = isActive;
        }

        public int[] CurrencyIds { get; init; }
        public bool IsActive { get; init; }
    }

    public sealed class AddRangeUserCurrencyFollowCommandHandler : ICommandHandler<AddRangeUserCurrencyFollowCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public AddRangeUserCurrencyFollowCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(AddRangeUserCurrencyFollowCommand command, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            IBaseResult result = await _unitOfWork
                .CurrencyRule
                .CheckAnyNotExistAsync(command.CurrencyIds, cancellationToken);

            if (!result.Success)
                return result;

            List<UserCurrencyFollow> toCreateData = new();

            int userId = _userTokenService.UserId;

            DateTime now = DateTime.UtcNow;

            foreach (var currencyId in command.CurrencyIds)
            {
                toCreateData.Add(new UserCurrencyFollow
                {
                    CurrencyId = currencyId,
                    UserId = userId,
                    UpdatedDate = now,
                    CreatedDate = now,
                    IsActive = command.IsActive
                });
            }

            if (toCreateData.Any())
            {
                await _unitOfWork.UserCurrencyFollowWriteRepository.AddRangeAsync(toCreateData, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new ResultDto(201, true);
        }
    }
}