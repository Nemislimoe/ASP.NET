using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab3.Pages
{
    public class NewsletterModel : PageModel
    {
        [BindProperty]
        public NewsletterInput Input { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public static List<string> Subscriptions { get; } = new List<string>();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Subscriptions.Contains(Input.Email))
            {
                Subscriptions.Add(Input.Email);
            }

            SuccessMessage = $"Підписка успішна для {Input.Email}";
            return RedirectToPage("Success");
        }
    }

    public class NewsletterInput
    {
        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; }
    }
}
