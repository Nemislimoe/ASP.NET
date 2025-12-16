using System.Collections.Generic;

namespace Lab17.DTOs
{
    public class UserDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }          // Рівень 1
        public AddressDto? Address { get; set; }   // Рівень 2
        public List<string>? Roles { get; set; }   // Рівень 3
    }
}
