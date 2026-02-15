using Application.Abstractions.Commons.Results;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.AIs;
using Application.Models.DTOs.Commons.Results;
using Application.Models.Enums;

namespace Adapter.Services.Externals
{
    public sealed class AITagResolver : IAITagResolver
    {
        private readonly IReadOnlyDictionary<AiAction, IAITagHandler> _handlerMap;

        public AITagResolver(IEnumerable<IAITagHandler> handlers)
        {
            _handlerMap = handlers.
                GroupBy(x=> x.AiAction).
                ToDictionary(g=> g.Key, g=> g.First());
        }
        public async Task<IBaseResult> ResolveAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default)
        {
            if(intent is null)
                return new ResultDto(400, false, null, "Intent is null");

            if(!_handlerMap.TryGetValue(intent.Action, out var handler))
                return new ResultDto(400, false, null, $"Unsupported action: {intent.Action}");

            return await handler.HandleAsync(intent, userId, cancellationToken);
        }
    }
}