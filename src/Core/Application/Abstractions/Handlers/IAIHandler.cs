using Application.Abstractions.Services.Externals;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IAIHandler
    {
        Task<IResult> Parse(string message, IAiService service, CancellationToken cancellationToken);
    }
}