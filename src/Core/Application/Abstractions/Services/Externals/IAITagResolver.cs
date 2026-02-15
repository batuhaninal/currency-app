using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.AIs;

namespace Application.Abstractions.Services.Externals
{
    public interface IAITagResolver
    {
        Task<IBaseResult> ResolveAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default);
    }
}