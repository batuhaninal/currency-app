using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Services.Externals
{
    public interface IAIOrchestrator
    {
        Task<IBaseResult> HandleAsync(string userMessage, int userId);
    }
}