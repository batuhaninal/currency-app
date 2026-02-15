using Application.Abstractions.Handlers;
using Application.Abstractions.Services.Externals;

namespace API.Handlers.v1
{
    public sealed class AIHandler : IAIHandler
    {
        public async Task<IResult> Parse(string message, IAiService service, CancellationToken cancellationToken)
        {
            var intent = await service.ParseAsync(message, cancellationToken);

            return Results.Ok(intent);
        }
    }
}