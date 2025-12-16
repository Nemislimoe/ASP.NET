namespace Lab15.Models
{
    public class OrderDto
    {
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public bool IncludeDetails { get; set; }
    }
}
