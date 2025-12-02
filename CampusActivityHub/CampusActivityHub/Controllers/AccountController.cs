using CampusActivityHub.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CampusActivityHub.Controllers;
public class AccountController : Controller
{
    private readonly AppDbContext _db;
    public AccountController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string returnUrl = "/")
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            ModelState.AddModelError("", "Користувача не знайдено.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("CookieAuth", principal);

        return LocalRedirect(returnUrl);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Profile()
    {
        var id = User.Identity?.Name;
        if (id == null) return RedirectToAction("Login");
        var user = await _db.Users.FindAsync(id);
        return View(user);
    }
}
