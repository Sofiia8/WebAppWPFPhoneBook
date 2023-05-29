using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApiIdentity.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
