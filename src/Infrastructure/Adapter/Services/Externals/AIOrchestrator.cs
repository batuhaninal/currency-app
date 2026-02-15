using Application.Abstractions.Commons.Results;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.Commons.Results;

namespace Adapter.Services.Externals
{
    public sealed class AIOrchestrator : IAIOrchestrator
    {
        private readonly IAiService _aiService;
        private readonly IAIExecutor _executor;

        public AIOrchestrator(IAiService aiService, IAIExecutor executor)
        {
            _aiService = aiService;
            _executor = executor;
        }

        public async Task<IBaseResult> ProcessAsync(string userInput, int userId, CancellationToken cancellationToken = default)
        {
            var intent = await _aiService.ParseAsync(userInput, cancellationToken);

            if (intent is null)
                return new ResultDto(400, false, null, "AI could not resolve intent");

            return await _executor.ExecuteAsync(intent, userId, cancellationToken);
        }
    }
}