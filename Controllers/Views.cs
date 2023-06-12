using APISolovki.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISolovki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Views : ControllerBase
    {
    
        private readonly PrisonSolovkiContext _context;

        public Views(PrisonSolovkiContext context)
        {
            _context = context;
        }

        // GET: api/Works
        [Authorize]
        [HttpGet("GetPrisoners")]
        public async Task<ActionResult<IEnumerable<PrisonerInfo>>> GetPrisonersView()
        {
            if (_context.PrisonerInfos == null)
            {
                return NotFound();
            }
            return await _context.PrisonerInfos.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductInfo>>> GetProducts()
        {
            if (_context.ProductInfos == null)
            {
                return NotFound();
            }
            return await _context.ProductInfos.ToListAsync();
        }
    }
}
