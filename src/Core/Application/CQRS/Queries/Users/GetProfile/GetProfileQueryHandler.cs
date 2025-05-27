using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
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

        public GetProfileQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(GetProfileQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = new ResultDto(401, false, null, ErrorMessage.UNAUTHORIZED);

            if (_userTokenService.IsAuthenticated)
            {
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
                    result = new ResultDto(200, true, profile);
                }

            }

            return result;
        }
    }
}