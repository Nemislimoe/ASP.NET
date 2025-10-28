using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages
{
    public class TemperatureConverterModel : PageModel
    {
        [BindProperty]
        public double? Celsius { get; set; }

        public double? Fahrenheit { get; private set; }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            if (Celsius.HasValue)
            {
                Fahrenheit = Celsius.Value * 9.0 / 5.0 + 32.0;
            }
        }
    }
}
