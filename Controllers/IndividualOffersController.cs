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
    public class IndividualOffersController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public IndividualOffersController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/IndividualOffers
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IndividualOffer>>> GetIndividualOffers()
        {
          if (_context.IndividualOffers == null)
          {
              return NotFound();
          }
            return await _context.IndividualOffers.ToListAsync();
        }

        // GET: api/IndividualOffers/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<IndividualOffer>> GetIndividualOffer(int? id)
        {
          if (_context.IndividualOffers == null)
          {
              return NotFound();
          }
            var individualOffer = await _context.IndividualOffers.FindAsync(id);

            if (individualOffer == null)
            {
                return NotFound();
            }

            return individualOffer;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<IndividualOffer>>>> GetPagesIndividualOffer(int Pages, int points)
        {
            var totalLogsCount = await _context.IndividualOffers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<IndividualOffer>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.IndividualOffers
                    .Where(log => log.IdIndividualOffers >= currentId)
                    .OrderBy(log => log.IdIndividualOffers)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<IndividualOffer>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdIndividualOffers + 1);
            }

            return answer;
        }

        // PUT: api/IndividualOffers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutIndividualOffer(int? id, IndividualOffer individualOffer)
        {
            if (id != individualOffer.IdIndividualOffers)
            {
                return BadRequest();
            }

            _context.Entry(individualOffer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndividualOfferExists(id))
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

        // POST: api/IndividualOffers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IndividualOffer>> PostIndividualOffer(IndividualOffer individualOffer)
        {
          if (_context.IndividualOffers == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.IndividualOffers'  is null.");
          }
            individualOffer.IdIndividualOffers = null;
            _context.IndividualOffers.Add(individualOffer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndividualOffer", new { id = individualOffer.IdIndividualOffers }, individualOffer);
        }

        // DELETE: api/IndividualOffers/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteIndividualOffer(int? id)
        {
            if (_context.IndividualOffers == null)
            {
                return NotFound();
            }
            var individualOffer = await _context.IndividualOffers.FindAsync(id);
            if (individualOffer == null)
            {
                return NotFound();
            }

            _context.IndividualOffers.Remove(individualOffer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IndividualOfferExists(int? id)
        {
            return (_context.IndividualOffers?.Any(e => e.IdIndividualOffers == id)).GetValueOrDefault();
        }
    }
}
