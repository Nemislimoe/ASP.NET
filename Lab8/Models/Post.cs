using System.ComponentModel.DataAnnotations;

namespace Lab8.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [StringLength(80)]
        public string Author { get; set; }
    }
}
