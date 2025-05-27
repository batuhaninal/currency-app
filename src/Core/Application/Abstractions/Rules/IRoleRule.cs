using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IRoleRule
    {
        Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckNameValidAsync(string title, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckNameValidAsync(int id, string title, CancellationToken cancellationToken = default);
    }
}