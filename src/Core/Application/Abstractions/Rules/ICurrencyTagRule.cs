using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface ICurrencyTagRule
    {
        Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckValueValidAsync(string value, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckValueValidAsync(int id, string value, CancellationToken cancellationToken = default);
    }
}