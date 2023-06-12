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
    public class TypeProductsController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public TypeProductsController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/TypeProducts
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeProduct>>> GetTypeProducts()
        {
          if (_context.TypeProducts == null)
          {
              return NotFound();
          }
            return await _context.TypeProducts.ToListAsync();
        }

        // GET: api/TypeProducts/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeProduct>> GetTypeProduct(int? id)
        {
          if (_context.TypeProducts == null)
          {
              return NotFound();
          }
            var typeProduct = await _context.TypeProducts.FindAsync(id);

            if (typeProduct == null)
            {
                return NotFound();
            }

            return typeProduct;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<TypeProduct>>>> GetPagesTypeProduct(int Pages, int points)
        {
            var totalLogsCount = await _context.TypeProducts.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<TypeProduct>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.TypeProducts
                    .Where(log => log.IdTypeProduct >= currentId)
                    .OrderBy(log => log.IdTypeProduct)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<TypeProduct>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdTypeProduct + 1);
            }

            return answer;
        }

        // PUT: api/TypeProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTypeProduct(int? id, TypeProduct typeProduct)
        {
            if (id != typeProduct.IdTypeProduct)
            {
                return BadRequest();
            }

            _context.Entry(typeProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeProductExists(id))
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

        // POST: api/TypeProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<TypeProduct>> PostTypeProduct(TypeProduct typeProduct)
        {
          if (_context.TypeProducts == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.TypeProducts'  is null.");
          }
            typeProduct.IdTypeProduct = null;
            _context.TypeProducts.Add(typeProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeProduct", new { id = typeProduct.IdTypeProduct }, typeProduct);
        }

        // DELETE: api/TypeProducts/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTypeProduct(int? id)
        {
            if (_context.TypeProducts == null)
            {
                return NotFound();
            }
            var typeProduct = await _context.TypeProducts.FindAsync(id);
            if (typeProduct == null)
            {
                return NotFound();
            }

            _context.TypeProducts.Remove(typeProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeProductExists(int? id)
        {
            return (_context.TypeProducts?.Any(e => e.IdTypeProduct == id)).GetValueOrDefault();
        }
    }
}
