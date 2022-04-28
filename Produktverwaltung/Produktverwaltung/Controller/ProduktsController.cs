#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Produktverwaltung.Models;

namespace Produktverwaltung.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduktsController : ControllerBase
    {
        private readonly ProduktContext _context;
        private readonly IConfiguration Configuration;
        private readonly ILogger<ProdukteBaseController> _logger;

        public ProduktsController(ProduktContext context, IConfiguration configuration, ILogger<ProdukteBaseController> logger)
        {
            _context = context;
            Configuration = configuration;
            _logger = logger;
        }

        // GET: api/Produkts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkt>>> GetProdukte()
        {
            _logger.LogInformation("Abruf aller Produkte");
            return await _context.Produkte.ToListAsync();
        }

        // GET: api/Produkts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produkt>> GetProdukt(long id)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende abrufen: {id}");
            var produkt = await _context.Produkte.FindAsync(id);

            if (produkt == null)
            {
                return NotFound();
            }

            return produkt;
        }

        // PUT: api/Produkts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdukt(long id, Produkt produkt)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende verändern: {produkt}");
            if (id != produkt.Id)
            {
                return BadRequest();
            }

            _context.Entry(produkt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduktExists(id))
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

        // POST: api/Produkts
        [HttpPost]
        public async Task<ActionResult<Produkt>> PostProdukt(Produkt produkt)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende speichern: {produkt}");
            _context.Produkte.Add(produkt);
            await _context.SaveChangesAsync();

            var idPrefix = Configuration["IDPrefix"];
            produkt.UniqueIdentifier = produkt.Id + idPrefix;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdukt", new { id = produkt.Id }, produkt);
        }

        // DELETE: api/Produkts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdukt(long id)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende löschen: {id}");
            var produkt = await _context.Produkte.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }

            _context.Produkte.Remove(produkt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduktExists(long id)
        {
            return _context.Produkte.Any(e => e.Id == id);
        }
    }
}
