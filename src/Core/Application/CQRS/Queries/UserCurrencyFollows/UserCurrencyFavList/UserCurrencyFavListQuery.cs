using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.UserCurrencyFollows.UserCurrencyFavList
{
    public sealed record UserCurrencyFavListQuery : IQuery
    {
        public UserCurrencyFavListQuery()
        {

        }

        public UserCurrencyFavListQuery(int? take, bool? isBroadcast, bool? all)
        {
            Take = take;
            IsBroadcast = isBroadcast;
            All = all;
        }

        public int? Take { get; set; }
        public bool? IsBroadcast { get; set; }
        public bool? All { get; set; }
    }

    public sealed class UserCurrencyFavListQueryHandler : IQueryHandler<UserCurrencyFavListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UserCurrencyFavListQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UserCurrencyFavListQuery query, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            int userId = _userTokenService.UserId;

            IBaseResult result = await _unitOfWork.UserRule.CheckExistAsync(userId, cancellationToken);

            if (query.All.HasValue && query.All.Value)
                {
                    if (query.IsBroadcast.HasValue && query.IsBroadcast.Value)
                    {
                        var broadcast = await _unitOfWork
                            .UserCurrencyFollowReadRepository
                            .Table
                            .AsNoTracking()
                            .Where(x => x.UserId == userId && x.IsActive)
                            .Select(x => x.CurrencyId)
                            .ToArrayAsync(cancellationToken);

                        return new ResultDto(200, true, broadcast);
                    }
                    else
                    {
                        var all = await _unitOfWork
                            .UserCurrencyFollowReadRepository
                            .Table
                            .AsNoTracking()
                            .Where(x => x.UserId == userId)
                            .Select(x => x.CurrencyId)
                            .ToArrayAsync(cancellationToken);

                        return new ResultDto(200, true, all);
                    }
                }

            if (query.IsBroadcast.HasValue && query.IsBroadcast.Value)
            {
                var broadcast = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.IsActive)
                .OrderByDescending(x => x.UpdatedDate)
                .Take(query.Take ?? 4)
                .Select(x => x.CurrencyId)
                .ToArrayAsync(cancellationToken);

                return new ResultDto(200, true, broadcast);
            }
            else
            {
                var data = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.UpdatedDate)
                .Take(query.Take ?? 4)
                .Select(x => x.CurrencyId)
                .ToArrayAsync(cancellationToken);

                return new ResultDto(200, true, data);
            }
        }
    }
}