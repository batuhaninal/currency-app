using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.AIs;

namespace Application.Abstractions.Services.Externals
{
    public interface IAIExecutor
    {
        Task<IBaseResult> ExecuteAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default);
    }
}