using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab2.Pages
{
    public class FeedbackModel : PageModel
    {
        [BindProperty]
        public string Comment { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(Comment))
            {
                ViewData["SavedComment"] = Comment;
            }
            else
            {
                ViewData["SavedComment"] = "Коментар порожній.";
            }
        }
    }
}
