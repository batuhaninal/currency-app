using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.Assets.Delete
{
    public sealed record DeleteAssetCommand : ICommand
    {
        public DeleteAssetCommand()
        {

        }

        public DeleteAssetCommand(int assetId)
        {
            AssetId = assetId;
        }


        public int AssetId { get; set; }
    }

    public sealed class DeleteAssetCommandHandler : ICommandHandler<DeleteAssetCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public DeleteAssetCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(DeleteAssetCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.AssetRule.CheckExistAsync(command.AssetId, _userTokenService.UserId, cancellationToken);

            if (!result.Success)
                return result;

            await _unitOfWork.AssetWriteRepository.RemoveAsync(command.AssetId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(200, true);
        }
    }
}