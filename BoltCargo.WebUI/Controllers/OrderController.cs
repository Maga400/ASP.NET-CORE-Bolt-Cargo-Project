using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // GET: api/<OrderController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<OrderDto>> Get()
        {
            var orders = await _orderService.GetAllAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto;
        }

        // GET api/<OrderController>/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order != null)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(new { order = orderDto });
            }
            return NotFound(new { message = "No feedback found with this id" });
        }

        [Authorize(Roles = "Client")]
        [HttpGet("clientOrders/{id}")]
        public async Task<IActionResult> GetClientOrders(string id)
        {
            var orders = await _orderService.GetByUserIdAsync(id);
            if (orders != null)
            {
                var ordersDto = _mapper.Map<List<OrderDto>>(orders);
                return Ok(new { orders = ordersDto });
            }
            return NotFound(new { message = "No user orders found with this id" });
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("driverOrders/{id}")]
        public async Task<IActionResult> GetDriverOrders(string id)
        {
            var orders = await _orderService.GetByDriverIdAsync(id);
            if (orders != null)
            {
                var ordersDto = _mapper.Map<List<OrderDto>>(orders);
                return Ok(new { orders = ordersDto });
            }
            return NotFound(new { message = "No driver orders found with this id" });
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("ordersWithCarType")]
        public async Task<IActionResult> GetOrdersWithCarTpe(string carType)
        {
            var orders = await _orderService.GetByCarTypeAsync(carType);
            if (orders != null)
            {
                var ordersDto = _mapper.Map<List<OrderDto>>(orders);
                return Ok(new { orders = ordersDto });
            }
            return NotFound(new { message = "No orders found with this car type" });
        }

        // POST api/<OrderController>
        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post([FromBody] OrderExtensionDto dto)
        {
            var order = _mapper.Map<Order>(dto);
            await _orderService.AddAsync(order);
            return Ok(new { message = "Order Added Successfully." });
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderUpdateDto dto)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order != null)
            {
                order.OrderAcceptedDate = dto.OrderAcceptedDate;
                order.IsAccept = dto.IsAccept;
                order.DriverId = dto.DriverId;
                 
                await _orderService.UpdateAsync(order);
                return Ok(new {Message = "Order Updated Successfully"});
            }

            return NotFound(new { message = "No order found with this id" });
        }

        // DELETE api/<OrderController>/5
        [Authorize(Roles = "Client,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order != null)
            {
                await _orderService.DeleteAsync(order);
                return Ok(new { message = "Order Deleted Successfully." });
            }
            return NotFound(new { message = "No order found with this id" });
        }
    }
}
