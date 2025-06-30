using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IUserAssetHistoryRule
    {
        Task<IBaseResult> CheckValidByGivenDate(int userId, DateOnly date, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckExist(int userAssetId, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckExist(int userAssetId, int userId, CancellationToken cancellationToken = default);
    }
}