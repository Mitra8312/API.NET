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
    public class FavoriteArticlesController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public FavoriteArticlesController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/FavoriteArticles
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteArticle>>> GetFavoriteArticles()
        {
          if (_context.FavoriteArticles == null)
          {
              return NotFound();
          }
            return await _context.FavoriteArticles.ToListAsync();
        }

        // GET: api/FavoriteArticles/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteArticle>> GetFavoriteArticle(int? id)
        {
          if (_context.FavoriteArticles == null)
          {
              return NotFound();
          }
            var favoriteArticle = await _context.FavoriteArticles.FindAsync(id);

            if (favoriteArticle == null)
            {
                return NotFound();
            }

            return favoriteArticle;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<FavoriteArticle>>>> GetPagesFavoriteArticle(int Pages, int points)
        {
            var totalLogsCount = await _context.Castes.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<FavoriteArticle>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.FavoriteArticles
                    .Where(log => log.IdFavoriteArticle >= currentId)
                    .OrderBy(log => log.IdFavoriteArticle)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<FavoriteArticle>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdFavoriteArticle + 1);
            }

            return answer;
        }

        // PUT: api/FavoriteArticles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavoriteArticle(int? id, FavoriteArticle favoriteArticle)
        {
            if (id != favoriteArticle.IdFavoriteArticle)
            {
                return BadRequest();
            }

            _context.Entry(favoriteArticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteArticleExists(id))
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

        // POST: api/FavoriteArticles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FavoriteArticle>> PostFavoriteArticle(FavoriteArticle favoriteArticle)
        {
          if (_context.FavoriteArticles == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.FavoriteArticles'  is null.");
          }
            favoriteArticle.IdFavoriteArticle = null;
            _context.FavoriteArticles.Add(favoriteArticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavoriteArticle", new { id = favoriteArticle.IdFavoriteArticle }, favoriteArticle);
        }

        // DELETE: api/FavoriteArticles/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavoriteArticle(int? id)
        {
            if (_context.FavoriteArticles == null)
            {
                return NotFound();
            }
            var favoriteArticle = await _context.FavoriteArticles.FindAsync(id);
            if (favoriteArticle == null)
            {
                return NotFound();
            }

            _context.FavoriteArticles.Remove(favoriteArticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavoriteArticleExists(int? id)
        {
            return (_context.FavoriteArticles?.Any(e => e.IdFavoriteArticle == id)).GetValueOrDefault();
        }
    }
}
