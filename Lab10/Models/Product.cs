using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Опис не може бути довшим за 100 символів")]
        public string Description { get; set; }

        [Range(1, 10000, ErrorMessage = "Ціна має бути від 1 до 10000")]
        public decimal Price { get; set; }
    }
}
