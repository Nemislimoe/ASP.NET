using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages
{
    public class CurrentTimeModel : PageModel
    {
        public DateTime CurrentDateTime { get; private set; }

        public void OnGet()
        {
            CurrentDateTime = DateTime.Now;
        }
    }
}
