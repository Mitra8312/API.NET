using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISolovki.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthsController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public HealthsController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Healths
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Health>>> GetHealths()
        {
          if (_context.Healths == null)
          {
              return NotFound();
          }
            return await _context.Healths.ToListAsync();
        }

        // GET: api/Healths/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Health>> GetHealth(int? id)
        {
          if (_context.Healths == null)
          {
              return NotFound();
          }
            var health = await _context.Healths.FindAsync(id);

            if (health == null)
            {
                return NotFound();
            }

            return health;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Health>>>> GetPagesHealth(int Pages, int points)
        {
            var totalLogsCount = await _context.Healths.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Health>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Healths
                    .Where(log => log.IdHealth >= currentId)
                    .OrderBy(log => log.IdHealth)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Health>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdHealth + 1);
            }

            return answer;
        }

        // PUT: api/Healths/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutHealth(int? id, Health health)
        {
            if (id != health.IdHealth)
            {
                return BadRequest();
            }

            _context.Entry(health).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HealthExists(id))
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

        // POST: api/Healths
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Health>> PostHealth(Health health)
        {
          if (_context.Healths == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Healths'  is null.");
          }
            health.IdHealth = null;
            _context.Healths.Add(health);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHealth", new { id = health.IdHealth }, health);
        }

        // DELETE: api/Healths/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHealth(int? id)
        {
            if (_context.Healths == null)
            {
                return NotFound();
            }
            var health = await _context.Healths.FindAsync(id);
            if (health == null)
            {
                return NotFound();
            }

            _context.Healths.Remove(health);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HealthExists(int? id)
        {
            return (_context.Healths?.Any(e => e.IdHealth == id)).GetValueOrDefault();
        }
    }
}
