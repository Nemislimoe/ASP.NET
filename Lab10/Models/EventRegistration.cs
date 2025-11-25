using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class EventRegistration
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище обов'язкове")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Вік обов'язковий")]
        [Range(18, 65, ErrorMessage = "Вік має бути від 18 до 65")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; }
    }
}
