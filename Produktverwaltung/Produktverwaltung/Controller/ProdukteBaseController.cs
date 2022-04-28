using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Produktverwaltung.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdukteBaseController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<ProdukteBaseController> _logger;

        public ProdukteBaseController(IConfiguration configuration, ILogger<ProdukteBaseController> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        // GET: api/<ProdukteController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Abruf aller Produkte");
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProdukteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende abrufen: {id}");
            return "value";
        }

        // POST api/<ProdukteController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende speichern: {value}");
        }

        // PUT api/<ProdukteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende verändern: {value}");
        }

        // DELETE api/<ProdukteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation($"Da will wohl jemand das folgende löschen: {id}");
        }
    }
}
