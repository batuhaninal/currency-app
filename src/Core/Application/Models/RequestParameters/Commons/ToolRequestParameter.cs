namespace Application.Models.RequestParameters.Commons
{
    public abstract class ToolRequestParameter
    {
        
        private string? _condition;
        public virtual string? Condition
        {
            get => _condition;
            set => _condition = value?.TrimStart().TrimEnd().ToLower();
        }
        public virtual string? OrderBy { get; set; }
    }
}