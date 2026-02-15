using Application.Models.DTOs.AIs;

namespace Application.Abstractions.Services.Externals
{
    public interface IAiService
    {
        Task<AiIntent> ParseAsync(string userMessage, CancellationToken cancellationToken= default);
    }
}