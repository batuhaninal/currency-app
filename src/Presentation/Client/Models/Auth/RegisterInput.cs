using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Auth
{
    public sealed record RegisterInput
    {
        public RegisterInput()
        {

        }

        public RegisterInput(string username, string password, string firstName, string lastName)
        {
            Username = username.TrimStart().TrimEnd();
            Password = password;
            FirstName = firstName.TrimStart().TrimEnd();
            LastName = lastName.TrimStart().TrimEnd();
        }

        [DisplayName("Kullanici Adi")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(10, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; init; } = null!;

        [DisplayName("Sifre")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(5, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        [DataType(DataType.Password)]
        public string Password { get; init; } = null!;

        [DisplayName("Sifre Tekrari")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(5, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Girmis oldugunuz sifreniz ile sifrenizin tekrar alanlari birbiriyle uyusmalidir.")]
        public string RepeatPassword { get; init; } = null!;

        [DisplayName("Isim")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(2, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        public string FirstName { get; init; } = null!;

        [DisplayName("Soyisim")]
        [Required(ErrorMessage = "{0} boş gecilmemelidir.")]
        [MaxLength(35, ErrorMessage = "{0} {1} karakterden buyuk olmamalidir.")]
        [MinLength(2, ErrorMessage = "{0} {1} kucuk olmamalidir.")]
        public string LastName { get; init; } = null!;
    }
}