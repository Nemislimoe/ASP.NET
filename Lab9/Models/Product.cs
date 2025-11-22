using System.ComponentModel.DataAnnotations;

namespace Lab9.Models
{
    public class Product
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
    }
}
