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
    public class NationsController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public NationsController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Nations
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nation>>> GetNations()
        {
          if (_context.Nations == null)
          {
              return NotFound();
          }
            return await _context.Nations.ToListAsync();
        }

        // GET: api/Nations/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Nation>> GetNation(int? id)
        {
          if (_context.Nations == null)
          {
              return NotFound();
          }
            var nation = await _context.Nations.FindAsync(id);

            if (nation == null)
            {
                return NotFound();
            }

            return nation;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Nation>>>> GetPagesNation(int Pages, int points)
        {
            var totalLogsCount = await _context.Nations.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Nation>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Nations
                    .Where(log => log.IdNation >= currentId)
                    .OrderBy(log => log.IdNation)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Nation>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdNation + 1);
            }

            return answer;
        }

        // PUT: api/Nations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutNation(int? id, Nation nation)
        {
            if (id != nation.IdNation)
            {
                return BadRequest();
            }

            _context.Entry(nation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NationExists(id))
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

        // POST: api/Nations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Nation>> PostNation(Nation nation)
        {
          if (_context.Nations == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Nations'  is null.");
          }
            nation.IdNation = null;
            _context.Nations.Add(nation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNation", new { id = nation.IdNation }, nation);
        }

        // DELETE: api/Nations/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNation(int? id)
        {
            if (_context.Nations == null)
            {
                return NotFound();
            }
            var nation = await _context.Nations.FindAsync(id);
            if (nation == null)
            {
                return NotFound();
            }

            _context.Nations.Remove(nation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NationExists(int? id)
        {
            return (_context.Nations?.Any(e => e.IdNation == id)).GetValueOrDefault();
        }
    }
}
