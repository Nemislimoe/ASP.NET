using Lab15.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab15.Controllers
{
    [ApiController]
    [Route("api/shops/{shopId}/users/{userId}/orders")]
    public class OrdersController : ControllerBase
    {
        // GET api/shops/{shopId}/users/{userId}/orders/{orderId}?includeDetails=...
        [HttpGet("{orderId}")]
        public IActionResult GetOrder(
            [FromRoute] int shopId,
            [FromRoute] int userId,
            [FromRoute] int orderId,
            [FromQuery] bool includeDetails = false)
        {
            var response = new
            {
                ShopId = shopId,
                UserId = userId,
                OrderId = orderId,
                IncludeDetails = includeDetails,
                Details = includeDetails ? new { Items = 3, Notes = "Extended info" } : null
            };
            return Ok(response);
        }
    }
}
