using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISolovki.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrisonersController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public PrisonersController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Prisoners
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prisoner>>> GetPrisoners()
        {
          if (_context.Prisoners == null)
          {
              return NotFound();
          }
            return await _context.Prisoners.ToListAsync();
        }


        // GET: api/Prisoners/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Prisoner>> GetPrisoner(int? id)
        {
          if (_context.Prisoners == null)
          {
              return NotFound();
          }
            var prisoner = await _context.Prisoners.FindAsync(id);

            if (prisoner == null)
            {
                return NotFound();
            }

            return prisoner;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Prisoner>>>> GetPagesIndividualOffer(int Pages, int points)
        {
            var totalLogsCount = await _context.Prisoners.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Prisoner>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Prisoners
                    .Where(log => log.IdPrisoner >= currentId)
                    .OrderBy(log => log.IdPrisoner)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Prisoner>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdPrisoner + 1);
            }

            return answer;
        }

        // PUT: api/Prisoners/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPrisoner(int? id, Prisoner prisoner)
        {
            if (id != prisoner.IdPrisoner)
            {
                return BadRequest();
            }

            _context.Entry(prisoner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrisonerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Prisoners
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Prisoner>> PostPrisoner(Prisoner prisoner)
        {
          if (_context.Prisoners == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Prisoners'  is null.");
          }
            prisoner.IdPrisoner = null;
            _context.Prisoners.Add(prisoner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrisoner", new { id = prisoner.IdPrisoner }, prisoner);
        }

        // DELETE: api/Prisoners/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePrisoner(int? id)
        {
            if (_context.Prisoners == null)
            {
                return NotFound();
            }
            var prisoner = await _context.Prisoners.FindAsync(id);
            if (prisoner == null)
            {
                return NotFound();
            }

            _context.Prisoners.Remove(prisoner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrisonerExists(int? id)
        {
            return (_context.Prisoners?.Any(e => e.IdPrisoner == id)).GetValueOrDefault();
        }
    }
}
