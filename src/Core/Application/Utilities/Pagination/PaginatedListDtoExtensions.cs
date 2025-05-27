using Application.Models.DTOs.Commons.Results;
using Application.Models.RequestParameters.Commons;

namespace Application.Utilities.Pagination
{
    public static class PaginatedListDtoExtensions
    {
        public static async Task<PaginatedListDto<T>> ToPaginatedListDtoAsync<T>(this IQueryable<T> source, int? pageIndex, int? pageSize, CancellationToken cancellationToken) =>
            await PaginatedListDto<T>.CreateAsync(source, pageIndex, pageSize, cancellationToken);
        
        public static async Task<PaginatedListDto<T>> ToPaginatedListDtoAsync<T>(this IQueryable<T> source, BaseRequestParameter parameter, CancellationToken cancellationToken) =>
            await PaginatedListDto<T>.CreateAsync(source, parameter.PageIndex, parameter.PageSize, cancellationToken);
    }
}
