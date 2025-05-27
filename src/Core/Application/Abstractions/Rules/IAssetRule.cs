using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IAssetRule
    {
        Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckExistAsync(int id, int userId, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckUserHasAnyCurrencyAsset(int currencyId, int userId, CancellationToken cancellationToken = default);
    }
}