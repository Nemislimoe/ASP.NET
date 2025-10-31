using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab2.Pages
{
    [BindProperties]
    [Route("Product/{id:int}")]
    public class ProductModel : PageModel
    {
        public int Id { get; set; }            // маршрутний параметр
        public string Color { get; set; }     // query параметр
        public int? Page { get; set; }        // query параметр

        public void OnGet([FromRoute] int id, [FromQuery] string color, [FromQuery] int? page)
        {
            Id = id;
            Color = color;
            Page = page;
        }
    }
}
