using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusActivityHub.Data;

[Route("api/tags")]
[ApiController]
public class TagsApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public TagsApiController(AppDbContext db) => _db = db;

    // GET api/tags/autocomplete?q=...
    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete(string q)
    {
        if (string.IsNullOrWhiteSpace(q)) return Ok(new string[0]);
        // For demo: tags stored as comma-separated in Events.Title or separate table in real app
        var tags = await _db.Events
            .Where(e => e.Title.Contains(q))
            .Select(e => e.Title)
            .Distinct()
            .Take(10)
            .ToListAsync();
        return Ok(tags);
    }
}
