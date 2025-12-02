using System.ComponentModel.DataAnnotations;

namespace Lab12.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Name { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Ціна має бути в діапазоні від 0.01 до 10000")]
        public decimal Price { get; set; }
    }
}
