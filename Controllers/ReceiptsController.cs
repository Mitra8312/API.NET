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
    public class ReceiptsController : ControllerBase
    {
        private readonly PrisonSolovkiContext _context;

        public ReceiptsController(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Receipts
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipts()
        {
          if (_context.Receipts == null)
          {
              return NotFound();
          }
            return await _context.Receipts.ToListAsync();
        }

        // GET: api/Receipts/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> GetReceipt(int? id)
        {
          if (_context.Receipts == null)
          {
              return NotFound();
          }
            var receipt = await _context.Receipts.FindAsync(id);

            if (receipt == null)
            {
                return NotFound();
            }

            return receipt;
        }

        [Authorize]
        [HttpGet("Pagination")]
        public async Task<ActionResult<List<List<Receipt>>>> GetPagesReceipt(int Pages, int points)
        {
            var totalLogsCount = await _context.Receipts.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / points);

            var answer = new List<List<Receipt>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Receipts
                    .Where(log => log.IdReceipt >= currentId)
                    .OrderBy(log => log.IdReceipt)
                    .Take(points)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Receipt>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < points)
                {
                    break;
                }

                currentId = (int)(logs.Last().IdReceipt + 1);
            }

            return answer;
        }

        // PUT: api/Receipts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutReceipt(int? id, Receipt receipt)
        {
            if (id != receipt.IdReceipt)
            {
                return BadRequest();
            }

            _context.Entry(receipt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiptExists(id))
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

        // POST: api/Receipts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Receipt>> PostReceipt(Receipt receipt)
        {
          if (_context.Receipts == null)
          {
              return Problem("Entity set 'PrisonSolovkiContext.Receipts'  is null.");
          }
            receipt.IdReceipt = null;
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReceipt", new { id = receipt.IdReceipt }, receipt);
        }

        // DELETE: api/Receipts/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReceipt(int? id)
        {
            if (_context.Receipts == null)
            {
                return NotFound();
            }
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            _context.Receipts.Remove(receipt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReceiptExists(int? id)
        {
            return (_context.Receipts?.Any(e => e.IdReceipt == id)).GetValueOrDefault();
        }
    }
}
