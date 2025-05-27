using Application.Abstractions.Commons.Results;
using Application.Models.Constants.Settings;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.Models.DTOs.Commons.Results
{
    public class PaginatedListDto<T> : IPaginatedDataResult<T>
    {
        [JsonConstructor]
        public PaginatedListDto()
        {
            
        }
        public PaginatedListDto(List<T> items, int count, int pageIndex, int pageSize) 
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            ItemsCount = items.Count;
            Items = items;
        }

        public int TotalCount { get; set; }

        public int ItemsCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get => PageIndex > 1; set { } }

        public bool HasNextPage { get => PageIndex < TotalPages; set { } }

        public IEnumerable<T> Items { get; set; }

        public static async Task<PaginatedListDto<T>> CreateAsync(IQueryable<T> query, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (!pageSize.HasValue)
                pageSize = 20;
                
            if (pageIndex < 1)
                pageIndex = 1;
            if (pageSize < 1 || pageSize > SettingConstant.PaginationSettings.MaxPageSize)
                pageSize = 20;

            int count = await query.CountAsync();
            var data = await query
                .Skip((pageIndex.Value-1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync(cancellationToken);

            return new PaginatedListDto<T>(data, count, pageIndex.Value, pageSize.Value);
        }
    }
} 
