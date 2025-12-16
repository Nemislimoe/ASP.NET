using Lab15.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab15.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto? login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(new { Message = "Username and password are required" });
            }

            // Демонстраційна логіка: username == "admin" && password == "password"
            if (login.Username == "admin" && login.Password == "password")
            {
                var token = new { Token = "fake-jwt-token", Username = login.Username };
                return Ok(token);
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }

        // POST api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Просто повертаємо повідомлення
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
