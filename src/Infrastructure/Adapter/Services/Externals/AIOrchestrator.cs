using Application.Abstractions.Commons.Results;
using Application.Abstractions.Services.Externals;
using Application.CQRS.Commons.Services;

namespace Adapter.Services.Externals
{
    public sealed class AIOrchestrator : IAIOrchestrator
    {
        private readonly IAiService _aiService;
        private readonly IAITagResolver _tagResolver;
        private readonly Dispatcher _dispatcher;
        public Task<IBaseResult> HandleAsync(string userMessage, int userId)
        {
            throw new NotImplementedException();
        }
    }
}