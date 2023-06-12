using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISolovki.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlePublishersController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public ArticlePublishersController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/ArticlePublishers
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticlePublisher>>> GetArticlePublishers()
        {
          if (_context.ArticlePublishers == null)
          {
              return NotFound();
          }
            return await _context.ArticlePublishers.ToListAsync();
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<ArticlePublisher>>>> GetPagesArticlePublisher(int Pages, int points)
        {
            var totalLogsCount = await _context.ArticlePublishers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<ArticlePublisher>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.ArticlePublishers
                    .Where(log => log.IdArticlePublisher >= currentId)
                    .OrderBy(log => log.IdArticlePublisher)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<ArticlePublisher>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdArticlePublisher + 1);
            }

            return answer;
        }

        // GET: api/ArticlePublishers/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticlePublisher>> GetArticlePublisher(int? id)
        {
          if (_context.ArticlePublishers == null)
          {
              return NotFound();
          }
            var articlePublisher = await _context.ArticlePublishers.FindAsync(id);

            if (articlePublisher == null)
            {
                return NotFound();
            }

            return articlePublisher;
        }

        // PUT: api/ArticlePublishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutArticlePublisher(int? id, ArticlePublisher articlePublisher)
        {
            if (id != articlePublisher.IdArticlePublisher)
            {
                return BadRequest();
            }

            _context.Entry(articlePublisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticlePublisherExists(id))
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

        // POST: api/ArticlePublishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ArticlePublisher>> PostArticlePublisher(ArticlePublisher articlePublisher)
        {
          if (_context.ArticlePublishers == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.ArticlePublishers'  is null.");
          }
            articlePublisher.IdArticlePublisher = null;
            _context.ArticlePublishers.Add(articlePublisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticlePublisher", new { id = articlePublisher.IdArticlePublisher }, articlePublisher);
        }

        // DELETE: api/ArticlePublishers/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticlePublisher(int? id)
        {
            if (_context.ArticlePublishers == null)
            {
                return NotFound();
            }
            var articlePublisher = await _context.ArticlePublishers.FindAsync(id);
            if (articlePublisher == null)
            {
                return NotFound();
            }

            _context.ArticlePublishers.Remove(articlePublisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticlePublisherExists(int? id)
        {
            return (_context.ArticlePublishers?.Any(e => e.IdArticlePublisher == id)).GetValueOrDefault();
        }
    }
}
