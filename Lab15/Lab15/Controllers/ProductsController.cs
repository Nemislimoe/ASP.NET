using Lab15.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab15.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // GET api/products/{categoryId}/{productId}
        [HttpGet("{categoryId}/{productId}")]
        public IActionResult GetProduct([FromRoute] int categoryId, [FromRoute] int productId)
        {
            var result = new
            {
                CategoryId = categoryId,
                ProductId = productId
            };
            return Ok(result);
        }

        // GET api/products/search?name=...&minPrice=...
        [HttpGet("search")]
        public IActionResult SearchProducts([FromQuery] string? name, [FromQuery] decimal? minPrice)
        {
            var result = new
            {
                Name = name,
                MinPrice = minPrice
            };
            return Ok(result);
        }

        // POST api/products
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto? product)
        {
            if (product == null)
                return BadRequest(new { Message = "Product body is required" });

            // У реальному додатку тут збереження в БД
            return CreatedAtAction(nameof(GetProduct),
                new { categoryId = product.CategoryId, productId = product.ProductId },
                product);
        }
    }
}
