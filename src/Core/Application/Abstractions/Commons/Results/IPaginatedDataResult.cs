namespace Application.Abstractions.Commons.Results
{
    public interface IPaginatedDataResult<T>
    {
        public int TotalCount { get; }
        public int ItemsCount { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
        public IEnumerable<T> Items { get; }
    }
}
