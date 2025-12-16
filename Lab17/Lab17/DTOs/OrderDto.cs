using System.Collections.Generic;

namespace Lab17.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public List<string> ProductNames { get; set; } = new List<string>();
    }
}
