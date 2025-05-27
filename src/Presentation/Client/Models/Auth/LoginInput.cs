using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Auth
{
    public sealed record LoginInput
    {
        public LoginInput()
        {
            
        }

        public LoginInput(string username, string password)
        {
            Username = username.TrimStart().TrimEnd();
            Password = password;
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
        public string Password { get; init; } = null!;
    }
}