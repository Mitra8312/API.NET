using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISolovki.Models;
using Microsoft.AspNetCore.Authorization;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastesController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public CastesController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Castes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caste>>> GetCastes()
        {
          if (_context.Castes == null)
          {
              return NotFound();
          }
            return await _context.Castes.ToListAsync();
        }

        // GET: api/Castes/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Caste>> GetCaste(int? id)
        {
          if (_context.Castes == null)
          {
              return NotFound();
          }
            var caste = await _context.Castes.FindAsync(id);

            if (caste == null)
            {
                return NotFound();
            }

            return caste;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Caste>>>> GetPagesCaste(int Pages, int points)
        {
            var totalLogsCount = await _context.Castes.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Caste>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Castes
                    .Where(log => log.IdCaste >= currentId)
                    .OrderBy(log => log.IdCaste)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Caste>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdCaste + 1);
            }

            return answer;
        }

        // PUT: api/Castes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCaste(int? id, Caste caste)
        {
            if (id != caste.IdCaste)
            {
                return BadRequest();
            }

            _context.Entry(caste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasteExists(id))
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

        // POST: api/Castes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Caste>> PostCaste(Caste caste)
        {
          if (_context.Castes == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Castes'  is null.");
          }
            caste.IdCaste = null;
            _context.Castes.Add(caste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaste", new { id = caste.IdCaste }, caste);
        }

        // DELETE: api/Castes/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCaste(int? id)
        {
            if (_context.Castes == null)
            {
                return NotFound();
            }
            var caste = await _context.Castes.FindAsync(id);
            if (caste == null)
            {
                return NotFound();
            }

            _context.Castes.Remove(caste);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CasteExists(int? id)
        {
            return (_context.Castes?.Any(e => e.IdCaste == id)).GetValueOrDefault();
        }
    }
}
