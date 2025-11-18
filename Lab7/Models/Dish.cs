namespace Lab7.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<DishCategory> DishCategories { get; set; } = new List<DishCategory>();
    }
}
