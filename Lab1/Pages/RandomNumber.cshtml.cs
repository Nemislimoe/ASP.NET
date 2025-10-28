using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages
{
    public class RandomNumberModel : PageModel
    {
        public int RandomNumber { get; private set; }

        public void OnGet()
        {
            Random rnd = new Random();
            RandomNumber = rnd.Next(1, 101);
        }
    }
}
