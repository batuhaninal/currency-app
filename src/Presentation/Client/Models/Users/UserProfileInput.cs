using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Users
{
    public sealed record UserProfileInput
    {
        public UserProfileInput()
        {

        }

        public UserProfileInput(string firstName, string lastName, string? oldPassword, string? newPassword, string? repeatPassword)
        {
            FirstName = firstName;
            LastName = lastName;
            OldPassword = oldPassword;
            NewPassword = newPassword;
            RepeatPassword = repeatPassword;
        }

        [DisplayName("Isim")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(2, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        public string FirstName { get; init; }

        [DisplayName("Soyisim")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(35, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(2, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        public string LastName { get; init; }

        [DisplayName("Eski Sifre")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(5, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        [DataType(DataType.Password)]
        public string? OldPassword { get; init; }

        [DisplayName("Yeni Sifre")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(5, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; init; }

        [DisplayName("Sifre Tekrari")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(5, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Girmis oldugunuz sifreniz ile sifrenizin tekrar alanlari birbiriyle uyusmalidir.")]
        public string? RepeatPassword { get; init; }
    }
}