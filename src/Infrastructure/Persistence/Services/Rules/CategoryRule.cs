using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Categories;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class CategoryRule : ICategoryRule
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public CategoryRule(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _categoryReadRepository.AnyAsync(x => x.Id == id, cancellationToken);
            
            return new ResultDto(400, result, null, ErrorMessage.CATEGORYEXIST);
        }

        public async Task<IBaseResult> CheckTitleValidAsync(string title, CancellationToken cancellationToken = default)
        {
            bool result = await _categoryReadRepository.AnyAsync(x=> x.Title.ToLower() == title.ToLower(), cancellationToken);

            return new ResultDto(400, !result, null, ErrorMessage.CATEGORYTITLEDUPLICATE);
        }

        public async Task<IBaseResult> CheckTitleValidAsync(int id, string title, CancellationToken cancellationToken = default)
        {
            bool result = await _categoryReadRepository.AnyAsync(x=> x.Id != id && x.Title.ToLower() == title.ToLower(), cancellationToken);

            return new ResultDto(400, !result, null, ErrorMessage.CATEGORYTITLEDUPLICATE);
        }
    }
}