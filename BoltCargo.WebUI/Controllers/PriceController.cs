using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoltCargo.WebUI.Controllers
{
    [Authorize(Roles = "Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PriceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<PriceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PriceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PriceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PriceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PriceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
