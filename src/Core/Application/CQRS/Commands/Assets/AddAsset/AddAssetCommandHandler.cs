using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Assets.AddAsset
{
    public sealed class AddAssetCommandHandler : ICommandHandler<AddAssetCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public AddAssetCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(AddAssetCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = new ResultDto(401, false, null, ErrorMessage.UNAUTHORIZED);

            if (_userTokenService.IsAuthenticated)
            {
                result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);

                if (result.Success)
                {
                    CurrencyPriceDto currencyPriceDto = (await _unitOfWork.CurrencyReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(x => x.Id == command.CurrencyId)
                        .Select(x => new CurrencyPriceDto(x.Id, x.PurchasePrice, x.SalePrice))
                        .FirstOrDefaultAsync(cancellationToken))!;

                    Asset asset = await _unitOfWork.AssetWriteRepository.CreateAsync(command.ToDomain(_userTokenService.UserId, currencyPriceDto.PurchasePrice, currencyPriceDto.SalePrice), cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    AssetInfoDto? data = await _unitOfWork.AssetReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(x => x.Id == asset.Id)
                        .Select(x => new AssetInfoDto(
                            x.Id,
                            x.Count,
                            x.PurchasePrice,
                            x.SalePrice,
                            x.CurrentPurchasePrice,
                            x.CurrentSalePrice,
                            x.PurchaseDate,
                            x.CreatedDate,
                            x.UpdatedDate,
                            x.IsActive,
                            x.Currency != null ? new CurrencyRelationDto(
                                x.Currency.Id,
                                x.Currency.Title,
                                x.Currency.SubTitle,
                                x.Currency.PurchasePrice,
                                x.Currency.SalePrice
                            ) : null,
                            x.User != null ? new UserRelationDto(
                                x.User.Id,
                                x.User.Email,
                                x.User.FirstName,
                                x.User.LastName
                            ) : null
                            ))
                            .FirstOrDefaultAsync(cancellationToken);

                    result = new ResultDto(200, true, data);
                }
            }

            return result;
        }
    }
}