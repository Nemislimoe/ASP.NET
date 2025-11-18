using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;

namespace Lab7.Pages.Participants
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public DetailsModel(AppDbContext context) => _context = context;

        public Participant Participant { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Participant = await _context.Participants
                .Include(p => p.EventParticipants)
                    .ThenInclude(ep => ep.Event)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Participant == null) return NotFound();
            return Page();
        }
    }
}
