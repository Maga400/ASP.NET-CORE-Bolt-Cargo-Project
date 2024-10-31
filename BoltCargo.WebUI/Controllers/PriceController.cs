using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoltCargo.WebUI.Controllers
{
    [Authorize(Roles = "Client")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PriceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("carPrice")]
        public IActionResult GetCarPrice(string carType)
        {
            var carPriceSection = _configuration.GetSection("CarPrice");

            if (carPriceSection != null)
            {
                var priceValue = carPriceSection[carType];

                if (int.TryParse(priceValue, out int price))
                {
                    return Ok(new { Price = price });
                }
            }

            return NotFound(new { Message = "Car type not found!" });
        }

        [HttpGet("roadPrice")]
        public IActionResult GetRoadPrice(string carType,int km)
        {
            var carPriceSection = _configuration.GetSection("RoadPrice");

            if (carPriceSection != null)
            {
                var priceValue = carPriceSection[carType];

                if (double.TryParse(priceValue, out double roadPrice))
                {
                    var totalPrice = roadPrice * km;
                    return Ok(new { Price = totalPrice });
                }
            } 
            
            return NotFound(new { Message = "Car type not found!" });
        }

        //// GET: api/<PriceController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<PriceController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<PriceController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<PriceController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<PriceController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
