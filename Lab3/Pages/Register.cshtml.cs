using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab3.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterInput Input { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Тут можна зберегти користувача у БД
            SuccessMessage = $"Реєстрація пройшла успішно. Ласкаво просимо, {Input.Name}!";

            return RedirectToPage("Success");
        }
    }

    public class RegisterInput
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [StringLength(50, ErrorMessage = "Максимум {1} символів")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути не менше {2} символів")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю обов'язкове")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Range(18, 100, ErrorMessage = "Вік має бути від {1} до {2}")]
        public int? Age { get; set; }
    }
}
