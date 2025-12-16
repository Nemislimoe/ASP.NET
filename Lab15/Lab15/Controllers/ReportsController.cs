using Lab15.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab15.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        // GET api/reports/{year}/{month}
        [HttpGet("{year}/{month}")]
        public IActionResult GetReport([FromRoute] int year, [FromRoute] int month)
        {
            var report = new ReportDto
            {
                Year = year,
                Month = month,
                TotalUsers = 1234,
                Revenue = 98765.43m
            };

            // Content negotiation: JSON or XML depending on Accept header
            return Ok(report);
        }
    }
}
