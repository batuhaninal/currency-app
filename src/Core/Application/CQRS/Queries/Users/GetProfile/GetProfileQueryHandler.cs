using System.Text.Json;
using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Users.GetProfile
{
    public sealed class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;
        private readonly ICacheService _cacheService;

        public GetProfileQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(GetProfileQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = new ResultDto(401, false, null, ErrorMessage.UNAUTHORIZED);

            if (_userTokenService.IsAuthenticated)
            {
                string cacheKey = CachePrefix.CreateByParameter(CachePrefix.UserPrefix, nameof(GetProfileQueryHandler), "email", _userTokenService.UserEmail);

                string? cacheData = await _cacheService.GetAsync(cacheKey);

                if(!string.IsNullOrEmpty(cacheData))
                {
                    var jsonData = JsonSerializer.Deserialize<UserProfileDto>(cacheData);

                    return new ResultDto(203, true, jsonData);
                }

                UserProfileDto? profile = await _unitOfWork.UserReadRepository.Table
                    .AsNoTracking()
                    .Where(x => x.Email == _userTokenService.UserEmail)
                    .Select(x => new UserProfileDto(
                        x.Id,
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        x.UserRoles.FirstOrDefault().Role.Name))
                    .FirstOrDefaultAsync(cancellationToken);

                if (profile != null)
                {
                    await _cacheService.AddAsync(cacheKey, profile);

                    result = new ResultDto(200, true, profile);
                }

            }

            return result;
        }
    }
}