using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class Feedback
    {
        [Required(ErrorMessage = "Заголовок обов'язковий")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Повідомлення обов'язкове")]
        public string Message { get; set; }
    }
}
