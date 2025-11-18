namespace Lab7.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DishCategory> DishCategories { get; set; } = new List<DishCategory>();
    }
}
