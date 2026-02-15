using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Services.Externals
{
    public interface IAIOrchestrator
    {
        Task<IBaseResult> ProcessAsync(string userInput, int userId, CancellationToken cancellationToken = default);
    }
}