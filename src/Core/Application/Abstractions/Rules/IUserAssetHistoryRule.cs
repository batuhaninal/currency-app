using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IUserAssetHistoryRule
    {
        Task<IBaseResult> CheckValidByGivenDate(int userId, DateOnly date, CancellationToken cancellationToken = default);
    }
}