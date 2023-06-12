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
    public class DangersController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public DangersController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Dangers
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Danger>>> GetDangers()
        {
          if (_context.Dangers == null)
          {
              return NotFound();
          }
            return await _context.Dangers.ToListAsync();
        }

        // GET: api/Dangers/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Danger>> GetDanger(int? id)
        {
          if (_context.Dangers == null)
          {
              return NotFound();
          }
            var danger = await _context.Dangers.FindAsync(id);

            if (danger == null)
            {
                return NotFound();
            }

            return danger;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Danger>>>> GetPagesDanger(int Pages, int points)
        {
            var totalLogsCount = await _context.Dangers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Danger>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Dangers
                    .Where(log => log.IdDanger >= currentId)
                    .OrderBy(log => log.IdDanger)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Danger>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdDanger + 1);
            }

            return answer;
        }

        // PUT: api/Dangers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutDanger(int? id, Danger danger)
        {
            if (id != danger.IdDanger)
            {
                return BadRequest();
            }

            _context.Entry(danger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DangerExists(id))
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

        // POST: api/Dangers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Danger>> PostDanger(Danger danger)
        {
          if (_context.Dangers == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Dangers'  is null.");
          }
            danger.IdDanger = null;
            _context.Dangers.Add(danger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDanger", new { id = danger.IdDanger }, danger);
        }

        // DELETE: api/Dangers/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDanger(int? id)
        {
            if (_context.Dangers == null)
            {
                return NotFound();
            }
            var danger = await _context.Dangers.FindAsync(id);
            if (danger == null)
            {
                return NotFound();
            }

            _context.Dangers.Remove(danger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DangerExists(int? id)
        {
            return (_context.Dangers?.Any(e => e.IdDanger == id)).GetValueOrDefault();
        }
    }
}
