using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.AIs;
using Application.Models.Enums;

namespace Application.Abstractions.Services.Externals
{
    public interface IAITagHandler
    {
        AiAction AiAction { get; }
        Task<IBaseResult> HandleAsync(
            AiIntent intent,
            int userId,
            CancellationToken cancellationToken = default
        );
    }
}