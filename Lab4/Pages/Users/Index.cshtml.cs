using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab4.Models;

namespace Lab4.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<User> Users { get; set; } = new();

        public void OnGet()
        {
            Users = new List<User> {
                new User { Id=1, Name="Anna", Email="anna@example.com", AvatarUrl="/images/avatar1.jpg"},
                new User { Id=2, Name="Ivan", Email="ivan@example.com", AvatarUrl="/images/avatar2.jpg"}
            };
        }
    }
}
