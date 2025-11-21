using System.ComponentModel.DataAnnotations;

namespace Lab8.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public bool IsCompleted { get; set; }
    }
}
