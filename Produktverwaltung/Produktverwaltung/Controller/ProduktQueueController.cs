using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Produktverwaltung.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduktQueueController : ControllerBase
    {
        // GET: api/<ProduktQueueController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProduktQueueController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProduktQueueController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProduktQueueController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProduktQueueController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
