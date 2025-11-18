namespace Lab7.Models
{
    public class DishCategory
    {
        public int DishId { get; set; }
        public Dish Dish { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
