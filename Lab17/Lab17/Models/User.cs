using System.Collections.Generic;
using System.Data;
using System.Net;

namespace Lab17.Models
{
    public class User
    {
        public int Id { get; set; }                 // Рівень 1
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Для рівнів 2-4
        public int Age { get; set; }
        public Address? Address { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
