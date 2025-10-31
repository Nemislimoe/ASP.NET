using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab2.Pages
{
    public class InfoModel : PageModel
    {
        public void OnGet()
        {
            ViewData["InfoMessage"] = "Це повідомлення передане через ViewData.";
            // або ViewBag.InfoMessage = "Повідомлення через ViewBag.";
        }
    }
}
