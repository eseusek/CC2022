#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Produktverwaltung.Helper;
using Produktverwaltung.Models;

namespace Produktverwaltung.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduktQueueController : ControllerBase
    {
        private readonly ProduktContext _context;
        private readonly IConfiguration Configuration;
        private readonly ILogger<ProdukteBaseController> _logger;

        public ProduktQueueController(ProduktContext context, IConfiguration configuration, ILogger<ProdukteBaseController> logger)
        {
            _context = context;
            Configuration = configuration;
            _logger = logger;
        }

        // GET: api/ProduktQueue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkt>>> GetProdukte()
        {
            return await _context.Produkte.ToListAsync();
        }

        // GET: api/ProduktQueue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produkt>> GetProdukt(long id)
        {
            var produkt = await _context.Produkte.FindAsync(id);

            if (produkt == null)
            {
                return NotFound();
            }

            return produkt;
        }

        // PUT: api/ProduktQueue/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdukt(long id, Produkt produkt)
        {
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

        // POST: api/ProduktQueue
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produkt>> PostProdukt(Produkt produkt)
        {
            _context.Produkte.Add(produkt);
            await _context.SaveChangesAsync();

            Bitmap img = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(img);

            // The font for our text
            Font f = new Font("Arial", 14);

            // work out how big the text will be when drawn as an image
            SizeF size = g.MeasureString(produkt.Name, f);

            // create a new Bitmap of the required size
            img = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
            g = Graphics.FromImage(img);

            // give it a white background
            g.Clear(Color.White);

            // draw the text in black
            g.DrawString(produkt.Name, f, Brushes.Black, 0, 0);

            img.Save(@$".\{produkt.Name}.jpg");

            BlobHelper bHelper = new BlobHelper(Configuration);

            QueueHelper qHelper = new QueueHelper(Configuration);

            bHelper.UploadDataToBlobContainer(Environment.CurrentDirectory, produkt.Name, "images");

            qHelper.CreateQueue("produkte");
            qHelper.InsertMessage("produkte", $"{produkt.Id} {produkt.Name} {produkt.Price} {produkt.Name}.jpg");

            return CreatedAtAction("GetProdukt", new { id = produkt.Id }, produkt);
        }

        // DELETE: api/ProduktQueue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdukt(long id)
        {
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
