namespace Client.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BreadcrumbAttribute : Attribute
    {
        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }
        public string Title { get; }
    }
}