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
    public class TypeOfActivitiesController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public TypeOfActivitiesController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/TypeOfActivities
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOfActivity>>> GetTypeOfActivities()
        {
          if (_context.TypeOfActivities == null)
          {
              return NotFound();
          }
            return await _context.TypeOfActivities.ToListAsync();
        }

        // GET: api/TypeOfActivities/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeOfActivity>> GetTypeOfActivity(int? id)
        {
          if (_context.TypeOfActivities == null)
          {
              return NotFound();
          }
            var typeOfActivity = await _context.TypeOfActivities.FindAsync(id);

            if (typeOfActivity == null)
            {
                return NotFound();
            }

            return typeOfActivity;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<TypeOfActivity>>>> GetPagesTypeOfActivity(int Pages, int points)
        {
            var totalLogsCount = await _context.TypeOfActivities.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<TypeOfActivity>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.TypeOfActivities
                    .Where(log => log.IdTypeOfActivity >= currentId)
                    .OrderBy(log => log.IdTypeOfActivity)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<TypeOfActivity>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdTypeOfActivity + 1);
            }

            return answer;
        }

        // PUT: api/TypeOfActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTypeOfActivity(int? id, TypeOfActivity typeOfActivity)
        {
            if (id != typeOfActivity.IdTypeOfActivity)
            {
                return BadRequest();
            }

            _context.Entry(typeOfActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeOfActivityExists(id))
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

        // POST: api/TypeOfActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<TypeOfActivity>> PostTypeOfActivity(TypeOfActivity typeOfActivity)
        {
          if (_context.TypeOfActivities == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.TypeOfActivities'  is null.");
          }
            typeOfActivity.IdTypeOfActivity = null;
            _context.TypeOfActivities.Add(typeOfActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeOfActivity", new { id = typeOfActivity.IdTypeOfActivity }, typeOfActivity);
        }

        // DELETE: api/TypeOfActivities/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTypeOfActivity(int? id)
        {
            if (_context.TypeOfActivities == null)
            {
                return NotFound();
            }
            var typeOfActivity = await _context.TypeOfActivities.FindAsync(id);
            if (typeOfActivity == null)
            {
                return NotFound();
            }

            _context.TypeOfActivities.Remove(typeOfActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeOfActivityExists(int? id)
        {
            return (_context.TypeOfActivities?.Any(e => e.IdTypeOfActivity == id)).GetValueOrDefault();
        }
    }
}
