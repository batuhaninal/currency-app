namespace Client.Models.Categories
{
    public sealed class CategoryFilterViewModel
    {
        public CategoryFilterViewModel()
        {

        }

        public CategoryFilterViewModel(bool isActive)
        {
            IsActive = isActive;
        }
        public bool IsActive { get; set; }
    }
}