using System.Collections.Generic;

namespace Lab17.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public User? User { get; set; }
        public List<OrderItem>? Items { get; set; }
    }
}
