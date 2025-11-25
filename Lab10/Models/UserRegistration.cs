using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше 6 символів")]
        public string Password { get; set; }
    }
}
