using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "ПІБ обов'язкове")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Повідомлення обов'язкове")]
        [StringLength(500, ErrorMessage = "Повідомлення не може перевищувати 500 символів")]
        public string Message { get; set; }
    }
}
