using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Security;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.Constants.Roles;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Commands.Users.Register
{
    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IHashingService hashingService, ITokenService tokenService, ILogger<RegisterCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<IBaseResult> Handle(RegisterCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.UserRule.CheckEmailValidAsync(command.Username, cancellationToken);

            if (result.Success)
            {
                byte[] pswHash, pswSalt;
                _hashingService.CreatePasswordHash(command.Password, out pswHash, out pswSalt);

                return await _unitOfWork.ExecuteWithRetryAsync<IBaseResult>(
                    async (uow, ct) =>
                    {
                        using (var tx = uow.BeginTransaction())
                        {
                            try
                            {
                                User user = await uow.UserWriteRepository.CreateAsync(command.ToUser(ref pswHash, ref pswSalt), ct);

                                await uow.SaveChangesAsync(ct);

                                UserRole role = await uow.UserRoleWriteRepository.CreateAsync(new UserRole
                                {
                                    RoleId = AppRoles.UserRoleId,
                                    UserId = user.Id,
                                });

                                await uow.SaveChangesAsync(ct);

                                user = (await uow.UserReadRepository.Table.AsNoTracking().Include(x => x.UserRoles!).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Email == command.Username))!;

                                var token = _tokenService.CreateAccessToken(user, 60 * 24 * 30);

                                await tx.CommitAsync(ct);

                                return new ResultDto(201, true, token);
                            }
                            catch (System.Exception ex)
                            {
                                _logger.LogError("{FunctionName} exception: {Exception}", typeof(RegisterCommandHandler).Name, ex);
                                await tx.RollbackAsync(ct);
                                return new ResultDto(500, false, null, ErrorMessage.INTERNAL);
                            }
                        }
                    },
                    cancellationToken
                );


            }

            return result;
        }
    }
}