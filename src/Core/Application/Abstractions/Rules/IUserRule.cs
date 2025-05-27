using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IUserRule
    {
        Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckExistAsync(string email, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckEmailValidAsync(string email, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckEmailValidAsync(int id, string email, CancellationToken cancellationToken = default);
    }
}