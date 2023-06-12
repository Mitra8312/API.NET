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
    public class WorksController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public WorksController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Works
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorks()
        {
          if (_context.Works == null)
          {
              return NotFound();
          }
            return await _context.Works.ToListAsync();
        }

        // GET: api/Works/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Work>> GetWork(int? id)
        {
          if (_context.Works == null)
          {
              return NotFound();
          }
            var work = await _context.Works.FindAsync(id);

            if (work == null)
            {
                return NotFound();
            }

            return work;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Work>>>> GetPagesWork(int Pages, int points)
        {
            var totalLogsCount = await _context.Works.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Work>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Works
                    .Where(log => log.IdWork >= currentId)
                    .OrderBy(log => log.IdWork)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Work>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdWork + 1);
            }

            return answer;
        }

        // PUT: api/Works/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutWork(int? id, Work work)
        {
            if (id != work.IdWork)
            {
                return BadRequest();
            }

            _context.Entry(work).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkExists(id))
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

        // POST: api/Works
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Work>> PostWork(Work work)
        {
          if (_context.Works == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Works'  is null.");
          }
            work.IdWork = null;
            _context.Works.Add(work);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWork", new { id = work.IdWork }, work);
        }

        // DELETE: api/Works/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWork(int? id)
        {
            if (_context.Works == null)
            {
                return NotFound();
            }
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }

            _context.Works.Remove(work);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkExists(int? id)
        {
            return (_context.Works?.Any(e => e.IdWork == id)).GetValueOrDefault();
        }
    }
}
