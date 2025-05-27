using System.Text.Json.Serialization;

namespace Application.Models.RequestParameters.Commons
{
    public abstract class BaseRequestParameter
    {
        private string? _condition;
        private int? _pageIndex;
        private int? _pageSize;
        public virtual string? Condition
        {
            get => _condition;
            set => _condition = value?.TrimStart().TrimEnd().ToLower();
        }
        public virtual string? OrderBy { get; set; }
        public virtual string? SortDirection { get; set; }
        public virtual bool? IsActive { get; set; }

        [JsonIgnore]
        protected virtual int MaxSize { get; set; } = 50;
        [JsonIgnore]
        protected virtual int DefaultSize { get; set; } = 20;

        public virtual int? PageIndex 
        { 
            get => _pageIndex <= 0 ? 1 : _pageIndex; 
            set => _pageIndex = value <= 0 ? 1 : value; 
        }
        public virtual int? PageSize 
        {
            get => _pageSize == 0 || _pageSize <= 0 ? DefaultSize : _pageSize; 
            set => _pageSize = (value <= 0 || value > MaxSize) ? DefaultSize : value; 
        }
    }
}