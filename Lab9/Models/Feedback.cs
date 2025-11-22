using System.ComponentModel.DataAnnotations;

namespace Lab9.Models
{
    public class Feedback
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Message { get; set; }
    }
}
