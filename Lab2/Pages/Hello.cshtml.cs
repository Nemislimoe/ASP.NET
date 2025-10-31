using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab2.Pages
{
    public class HelloModel : PageModel
    {
        public string Message { get; private set; }

        public void OnGet()
        {
            Message = "Привіт, студент!";
        }
    }
}
