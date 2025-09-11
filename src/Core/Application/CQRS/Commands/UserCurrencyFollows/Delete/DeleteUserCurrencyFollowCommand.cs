using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.UserCurrencyFollows.Delete
{
    public sealed record DeleteUserCurrencyFollowCommand : ICommand
    {
        public DeleteUserCurrencyFollowCommand()
        {

        }

        public DeleteUserCurrencyFollowCommand(int currencyId)
        {
            CurrencyId = currencyId;
        }

        public int CurrencyId { get; init; }
    }

    public sealed class DeleteUserCurrencyFollowCommandHandler : ICommandHandler<DeleteUserCurrencyFollowCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public DeleteUserCurrencyFollowCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(DeleteUserCurrencyFollowCommand command, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            int userId = _userTokenService.UserId;

            IBaseResult result = await _unitOfWork.UserRule.CheckExistAsync(userId, cancellationToken);

            if (!result.Success)
                return result;

            result = await _unitOfWork
                .UserCurrencyFollowRule
                .CheckUserFollowCurrency(userId, command.CurrencyId, cancellationToken);

            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CurrencyId == command.CurrencyId);

            if (data is not null)
            {
                _unitOfWork.UserCurrencyFollowWriteRepository.Remove(data);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new ResultDto(200, true);
        }
    }
}