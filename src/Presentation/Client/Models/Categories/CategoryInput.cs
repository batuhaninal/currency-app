using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Categories
{
    public sealed class CategoryInput
    {
        public CategoryInput()
        {

        }

        public CategoryInput(string title, bool isActive)
        {
            Title = title;
            IsActive = isActive;
        }

        [DisplayName("Kategori Adi")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(2, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        public string Title { get; init; } = null!;

        [DisplayName("Aktif")]
        public bool IsActive { get; init; }
    }
}