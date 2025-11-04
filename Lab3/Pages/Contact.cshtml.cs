using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab3.Pages
{
    public class ContactModel : PageModel
    {
        public static List<ContactEntry> InMemoryContacts { get; } = new List<ContactEntry>();

        [BindProperty]
        public ContactInput Input { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            InMemoryContacts.Add(new ContactEntry
            {
                FullName = Input.FullName,
                Email = Input.Email,
                Message = Input.Message
            });

            return RedirectToPage("ThankYou");
        }
    }

    public class ContactInput
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [StringLength(100, ErrorMessage = "Максимум {1} символів")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Повідомлення обов'язкове")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Повідомлення має бути мінімум {2} символів")]
        public string Message { get; set; }
    }

    public class ContactEntry
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
