using CampusActivityHubPRO.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusActivityHubPRO.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly SignInManager<ApplicationUser> _signIn;

        public AccountController(UserManager<ApplicationUser> user, SignInManager<ApplicationUser> signIn)
        {
            _user = user;
            _signIn = signIn;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var u = new ApplicationUser { UserName = email, Email = email, Name = name, EmailConfirmed = true };
            var res = await _user.CreateAsync(u, password);
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", string.Join("; ", res.Errors.Select(e => e.Description)));
                return View();
            }
            await _user.AddToRoleAsync(u, "Student");
            await _signIn.SignInAsync(u, isPersistent: false);
            return RedirectToAction("Index", "Events");
        }

        public IActionResult Login(string? returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            var res = await _signIn.PasswordSignInAsync(email, password, false, false);
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login");
                return View();
            }
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Events");
        }

        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Events");
        }
    }
}
