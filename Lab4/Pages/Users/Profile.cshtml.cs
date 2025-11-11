using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab4.Models;

namespace Lab4.Pages.Users
{
    public class ProfileModel : PageModel
    {
        public User CurrentUser { get; set; } = new();

        public void OnGet()
        {
            CurrentUser = new User { Id = 1, Name = "Anna", Email = "anna@example.com", AvatarUrl = "/images/avatar1.jpg" };
        }
    }
}
