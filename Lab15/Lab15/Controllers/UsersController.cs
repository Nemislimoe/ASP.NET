using Lab15.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab15.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // POST api/users/{id}/activate
        [HttpPost("{id}/activate")]
        public IActionResult ActivateUser([FromRoute] int id)
        {
            // Тут логіка активації (демо)
            var result = new { Id = id, Activated = true };
            return Ok(result);
        }

        // POST api/users/{id}/reset-password
        [HttpPost("{id}/reset-password")]
        public IActionResult ResetPassword([FromRoute] int id)
        {
            // Тут логіка скидання пароля (демо)
            var result = new { Id = id, PasswordReset = true, NewPassword = "temporary123" };
            return Ok(result);
        }

        // GET api/users/search?term=...
        [HttpGet("search")]
        public IActionResult SearchUsers([FromQuery] string? term)
        {
            var users = new[]
            {
                new UserDto { Id = 1, Name = "Ivan", IsActive = true },
                new UserDto { Id = 2, Name = "Iryna", IsActive = false },
                new UserDto { Id = 3, Name = "Petro", IsActive = true }
            }.Where(u => string.IsNullOrEmpty(term) || u.Name?.Contains(term, StringComparison.OrdinalIgnoreCase) == true);

            return Ok(users);
        }
    }
}
