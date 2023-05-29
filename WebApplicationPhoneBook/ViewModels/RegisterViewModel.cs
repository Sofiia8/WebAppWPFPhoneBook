using System.ComponentModel.DataAnnotations;

namespace WebApplicationPhoneBook.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(20)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }
    }
}
