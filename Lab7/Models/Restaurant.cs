namespace Lab7.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
