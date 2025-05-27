using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Roles;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class RoleRule : IRoleRule
    {
        private readonly IRoleReadRepository _roleReadRepository;

        public RoleRule(IRoleReadRepository roleReadRepository)
        {
            _roleReadRepository = roleReadRepository;
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _roleReadRepository.AnyAsync(x=> x.Id == id, cancellationToken);

            return new ResultDto(400, result, null, ErrorMessage.ROLEEXIST);
        }

        public async Task<IBaseResult> CheckNameValidAsync(string title, CancellationToken cancellationToken = default)
        {
            bool result = await _roleReadRepository.AnyAsync(x=> x.Name.ToLower() == title.ToLower(), cancellationToken);

            return new ResultDto(400, !result, null, ErrorMessage.ROLETITLEDUPLICATE);
        }

        public async Task<IBaseResult> CheckNameValidAsync(int id, string title, CancellationToken cancellationToken = default)
        {
            bool result = await _roleReadRepository.AnyAsync(x=> x.Id != id && x.Name.ToLower() == title.ToLower(), cancellationToken);

            return new ResultDto(400, !result, null, ErrorMessage.ROLETITLEDUPLICATE);
        }
    }
}