using Application.Abstractions.Commons.Results;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.AIs;
using Application.Models.DTOs.Commons.Results;
using Application.Models.Enums;

namespace Adapter.Services.Externals
{
    public sealed class UnkownTagHandler : IAITagHandler
    {
        public AiAction Action => AiAction.Unkown;
        public AiAction AiAction => throw new NotImplementedException();

        public async Task<IBaseResult> HandleAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default) => 
            new ResultDto(400, false, null, "Mesaj anlaşılamadı. Lütfen daha açık yazınız.");
    }
}