using Microsoft.AspNetCore.Mvc;
using Lab14.Models;

namespace Lab14.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputerGamesController : ControllerBase
    {
        // Тимчасове зберігання в пам'яті
        private static readonly List<ComputerGame> _games = new List<ComputerGame>
        {
            new ComputerGame { Id = 1, Name = "Space Odyssey", Price = 29.99m, ReleaseDate = new DateTime(2021, 5, 10) },
            new ComputerGame { Id = 2, Name = "Mystic Quest", Price = 49.50m, ReleaseDate = new DateTime(2022, 11, 1) },
            new ComputerGame { Id = 3, Name = "Racing Pro", Price = 19.00m, ReleaseDate = new DateTime(2020, 3, 15) }
        };

        // GET api/ComputerGames
        [HttpGet]
        public ActionResult<IEnumerable<ComputerGame>> GetAll()
        {
            return Ok(_games);
        }

        // GET api/ComputerGames/5
        [HttpGet("{id:int}")]
        public ActionResult<ComputerGame> GetById(int id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        // POST api/ComputerGames
        [HttpPost]
        public ActionResult<ComputerGame> Create([FromBody] ComputerGame newGame)
        {
            if (newGame == null) return BadRequest();

            // Проста логіка генерації Id
            var nextId = _games.Any() ? _games.Max(g => g.Id) + 1 : 1;
            newGame.Id = nextId;
            _games.Add(newGame);

            return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, newGame);
        }

        // PUT api/ComputerGames/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ComputerGame updatedGame)
        {
            if (updatedGame == null || id != updatedGame.Id) return BadRequest();

            var existing = _games.FirstOrDefault(g => g.Id == id);
            if (existing == null) return NotFound();

            existing.Name = updatedGame.Name;
            existing.Price = updatedGame.Price;
            existing.ReleaseDate = updatedGame.ReleaseDate;

            return NoContent();
        }

        // DELETE api/ComputerGames/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = _games.FirstOrDefault(g => g.Id == id);
            if (existing == null) return NotFound();

            _games.Remove(existing);
            return NoContent();
        }
    }
}
