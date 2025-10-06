using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Utilities.Helpers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Tools.ToolWithoutFavs
{
    public sealed record CurrencyToolWithoutFavsQuery : IQuery
    {
        public CurrencyToolWithoutFavsQuery()
        {

        }

        public CurrencyToolWithoutFavsQuery(bool isBroadcast)
        {
            IsBroadcast = isBroadcast;
        }
        public bool IsBroadcast { get; init; }
    }

    public sealed record ToolWithoutFavsQueryHandler : IQueryHandler<CurrencyToolWithoutFavsQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public ToolWithoutFavsQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(CurrencyToolWithoutFavsQuery query, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            int userId = _userTokenService.UserId;

            var favIds = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.IsActive == query.IsBroadcast && x.UserId == userId)
                .Select(x => x.CurrencyId)
                .ToListAsync(cancellationToken);

            var predicate = PredicateBuilderHelper.True<Currency>();

            if (!_userTokenService.IsAdmin)
                predicate = predicate.And(x => x.IsActive && x.Category != null && x.Category.IsActive);

            predicate = predicate.And(x => !favIds.Contains(x.Id));

            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(predicate)
                .Select(x => new CurrencyToolDto(
                    x.Id,
                    x.Title,
                    x.SubTitle,
                    x.IsActive
                ))
                .ToListAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}