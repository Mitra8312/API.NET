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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleOfTheConclusionsController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public ArticleOfTheConclusionsController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/ArticleOfTheConclusions
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleOfTheConclusion>>> GetArticleOfTheConclusions()
        {
          if (_context.ArticleOfTheConclusions == null)
          {
              return NotFound();
          }
            return await _context.ArticleOfTheConclusions.ToListAsync();
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<ArticleOfTheConclusion>>>> GetPagesArticleOfTheConclusion(int Pages, int points)
        {
            var totalLogsCount = await _context.ArticleOfTheConclusions.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<ArticleOfTheConclusion>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.ArticleOfTheConclusions
                    .Where(log => log.IdAotc >= currentId)
                    .OrderBy(log => log.IdAotc)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<ArticleOfTheConclusion>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdAotc + 1);
            }

            return answer;
        }

        // GET: api/ArticleOfTheConclusions/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleOfTheConclusion>> GetArticleOfTheConclusion(int? id)
        {
          if (_context.ArticleOfTheConclusions == null)
          {
              return NotFound();
          }
            var articleOfTheConclusion = await _context.ArticleOfTheConclusions.FindAsync(id);

            if (articleOfTheConclusion == null)
            {
                return NotFound();
            }

            return articleOfTheConclusion;
        }

        // PUT: api/ArticleOfTheConclusions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutArticleOfTheConclusion(int? id, ArticleOfTheConclusion articleOfTheConclusion)
        {
            if (id != articleOfTheConclusion.IdAotc)
            {
                return BadRequest();
            }

            _context.Entry(articleOfTheConclusion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleOfTheConclusionExists(id))
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

        // POST: api/ArticleOfTheConclusions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ArticleOfTheConclusion>> PostArticleOfTheConclusion(ArticleOfTheConclusion articleOfTheConclusion)
        {
          if (_context.ArticleOfTheConclusions == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.ArticleOfTheConclusions'  is null.");
          }
            articleOfTheConclusion.IdAotc = null;
            _context.ArticleOfTheConclusions.Add(articleOfTheConclusion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticleOfTheConclusion", new { id = articleOfTheConclusion.IdAotc }, articleOfTheConclusion);
        }

        // DELETE: api/ArticleOfTheConclusions/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticleOfTheConclusion(int? id)
        {
            if (_context.ArticleOfTheConclusions == null)
            {
                return NotFound();
            }
            var articleOfTheConclusion = await _context.ArticleOfTheConclusions.FindAsync(id);
            if (articleOfTheConclusion == null)
            {
                return NotFound();
            }

            _context.ArticleOfTheConclusions.Remove(articleOfTheConclusion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleOfTheConclusionExists(int? id)
        {
            return (_context.ArticleOfTheConclusions?.Any(e => e.IdAotc == id)).GetValueOrDefault();
        }
    }
}
