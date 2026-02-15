using Application.Abstractions.Commons.Results;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.AIs;
using Application.Models.DTOs.Commons.Results;

namespace Adapter.Services.Externals
{
    public sealed class AIExecutor : IAIExecutor
    {
        private readonly IEnumerable<IAITagHandler> _handlers;

        public AIExecutor(IEnumerable<IAITagHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task<IBaseResult> ExecuteAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default)
        {
            var handler = _handlers.FirstOrDefault(x => x.AiAction == intent.Action);

            if (handler is null)
                return new ResultDto(400, false, null, "Handler not found");

            return await handler.HandleAsync(intent, userId, cancellationToken);
        }
    }
}