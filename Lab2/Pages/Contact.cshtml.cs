using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab2.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        public string Greeting { get; private set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                Greeting = $"Привіт, {Name}!";
            }
            else
            {
                Greeting = "Привіт!";
            }
        }
    }
}
