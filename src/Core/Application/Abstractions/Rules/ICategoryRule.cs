using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface ICategoryRule
    {
        Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckTitleValidAsync(string title, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckTitleValidAsync(int id, string title, CancellationToken cancellationToken = default);
    }
}