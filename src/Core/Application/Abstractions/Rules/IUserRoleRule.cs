using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface IUserRoleRule
    {
        Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckUserRoleValidForUserAsync(int userId, int roleId, CancellationToken cancellationToken = default);
    }
}