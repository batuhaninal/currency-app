namespace Client.Models.Commons
{
    public sealed class PaginationResult<T> : PaginationBaseResult
    {
        public PaginationResult()
        {
            Items = new();
        }

        public List<T> Items { get; set; }
    }

    public abstract class PaginationBaseResult
    {
        public virtual int TotalCount { get; set; }

        public virtual int ItemsCount { get; set; }

        public virtual int PageIndex { get; set; } = 1;

        public virtual int PageSize { get; set; } = 20;

        public virtual int TotalPages { get; set; }

        public virtual bool HasPreviousPage { get => PageIndex > 1; set { } }

        public virtual bool HasNextPage { get => PageIndex < TotalPages; set { } }
    }
}