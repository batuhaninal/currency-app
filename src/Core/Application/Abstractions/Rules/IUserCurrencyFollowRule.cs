using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IUserCurrencyFollowRule
    {
        Task<IBaseResult> CheckExistAsync(int id, int userId, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckUserFollowValidAsync(int userId, int currencyId, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckUserFollowCurrency(int userId, int currencyId, CancellationToken cancellationToken = default);
        // Task<IBaseResult> CheckUserFollowCountValid(int userId, CancellationToken cancellationToken= default);
    }
}