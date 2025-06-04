using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.Assets.Update
{
    public sealed class UpdateAssetCommandHandler : ICommandHandler<UpdateAssetCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService  _userTokenService;

        public UpdateAssetCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UpdateAssetCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.AssetRule.CheckExistAsync(command.AssetId, _userTokenService.UserId, cancellationToken);

            if (!result.Success)
                return result;

            var asset = await _unitOfWork
                .AssetReadRepository
                .GetByIdAsync(command.AssetId, true, cancellationToken);

            asset!.PurchasePrice = command.PurchasePrice;
            asset.SalePrice = command.SalePrice;
            asset.Count = command.Count;
            asset.PurchaseDate = command.PurchaseDate;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(203, true, new AssetForUpdateDto(asset.Id, asset.Count, asset.PurchasePrice, asset.SalePrice, asset.PurchaseDate)); 
        }
    }
}