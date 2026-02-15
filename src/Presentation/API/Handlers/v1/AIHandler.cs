using Application.Abstractions.Handlers;
using Application.Abstractions.Services.Externals;

namespace API.Handlers.v1
{
    public sealed class AIHandler : IAIHandler
    {
        public async Task<IResult> Parse(string message, int userId, IAIOrchestrator orchestrator, CancellationToken cancellationToken)
        {
            var result = await orchestrator.ProcessAsync(message, userId, cancellationToken);

            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}