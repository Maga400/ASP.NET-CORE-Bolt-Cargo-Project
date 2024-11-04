using BoltCargo.Business.Services.Abstracts;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoltCargo.WebUI.Controllers
{
    [Authorize(Roles = "Client,Driver")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        public PriceController(IConfiguration configuration, IOrderService orderService)
        {
            _configuration = configuration;
            _orderService = orderService;
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
        public IActionResult GetRoadPrice(string carType,double km)
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

        [Authorize(Roles = "Driver")]
        [HttpGet("balance/{id}")]
        public async Task<IActionResult> GetBalance(string id)
        {
            var orders = await _orderService.GetDriverFinishedOrdersAsync(id);
            if (orders != null)
            {
                var totalPrice = orders.Sum(o => o.TotalPrice);
                var totalRoadPrice = orders.Sum(o => o.RoadPrice);
                var totalCarPrice = orders.Sum(o => o.CarPrice);
                var ordersCount = orders.Count;
                return Ok(new { TotalPrice = totalPrice, TotalRoadPrice = totalRoadPrice, TotalCarPrice = totalCarPrice, OrdersCount = ordersCount });
            }
            return NotFound(new { message = "No user orders found with this id" });
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
