﻿using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BoltCargo.WebUI.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly ICustomIdentityUserService _customIdentityUserService;

        public OrderController(IOrderService orderService, IMapper mapper, ICustomIdentityUserService customIdentityUserService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _customIdentityUserService = customIdentityUserService;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<List<OrderDto>> Get()
        {
            var orders = await _orderService.GetAllAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto;
        }

        // GET api/<OrderController>/5
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

        [Authorize(Roles = "Client")]
        [HttpGet("clientFinishedOrders/{id}")]
        public async Task<IActionResult> GetClientFinishedOrders(string id)
        {
            var orders = await _orderService.GetClientFinishedOrdersAsync(id);
            if (orders != null)
            {
                var ordersDto = _mapper.Map<List<OrderDto>>(orders);
                return Ok(new { orders = ordersDto });
            }
            return NotFound(new { message = "No user finished orders found with this id" });
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
        [HttpGet("driverFinishedOrders/{id}")] 
        public async Task<IActionResult> GetDriverFinishedOrders(string id)
        {
            var orders = await _orderService.GetDriverFinishedOrdersAsync(id);
            if (orders != null)
            {
                var ordersDto = _mapper.Map<List<OrderDto>>(orders);
                return Ok(new { orders = ordersDto });
            }
            return NotFound(new { message = "No driver finished orders found with this id" });
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

        [Authorize(Roles = "Admin")]
        [HttpGet("acceptedOrders")]
        public async Task<List<OrderDto>> GetAcceptedOrders()
        {
            var orders = await _orderService.GetAcceptedOrdersAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("unAcceptedOrders")]
        public async Task<List<OrderDto>> GetUnAcceptedOrders()
        {
            var orders = await _orderService.GetUnAcceptedOrdersAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("finishedOrders")]
        public async Task<List<OrderDto>> GetFinishedOrders()
        {
            var orders = await _orderService.GetFinishedOrdersAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return ordersDto; 
        } 

        // POST api/<OrderController>
        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderExtensionDto dto)
        {
            //var user = _mapper.Map<CustomIdentityUser>(dto.UserDto);
            //order.User = user; 
            var user = await _customIdentityUserService.GetByIdAsync(dto.UserId);
            var order = _mapper.Map<Order>(dto);
            order.User = user;
            await _orderService.AddAsync(order);
            return Ok(new { message = "Order Added Successfully." });
        }

        // PUT api/<OrderController>/5
        [Authorize(Roles = "Driver")]
        [HttpPut("acceptedOrder/{id}")]
        public async Task<IActionResult> AcceptOrder(int id, [FromBody] OrderUpdateDto dto)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order != null) 

            {
                order.OrderAcceptedDate = dto.OrderAcceptedDate;
                order.IsAccept = dto.IsAccept;
                order.DriverId = dto.DriverId;
                //order.Driver = dto.Driver;

                await _orderService.UpdateAsync(order);
                return Ok(new { Message = "Order Updated Successfully" });
            }

            return NotFound(new { message = "No order found with this id" });
        }

        //[Authorize(Roles = "Driver")]
        //[HttpPut("finishedOrder/{id}")]
        //public async Task<IActionResult> FinishOrder(int id, [FromBody] OrderFinishDto dto)
        //{
        //    var order = await _orderService.GetByIdAsync(id);
        //    if (order != null)

        //    {
        //        order.IsFinish = dto.IsFinish;

        //        await _orderService.UpdateAsync(order);
        //        return Ok(new { Message = "Order Updated Successfully" });
        //    }

        //    return NotFound(new { message = "No order found with this id" });
        //}

        [Authorize(Roles = "Driver")]
        [HttpPut("driverFinished/{id}")]
        public async Task<IActionResult> FinishOrder(int id, [FromBody] OrderFinishDto dto)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order != null)

            {
                order.IsDriverFinish = dto.IsFinish;

                if(order.IsDriverFinish && order.IsClientFinish) 
                {
                    order.IsFinish = true;
                }

                await _orderService.UpdateAsync(order);
                return Ok(new { Message = "Order Updated Successfully" });
            }

            return NotFound(new { message = "No order found with this id" });
        }

        [Authorize(Roles = "Client")]
        [HttpPut("clientFinished/{id}")]
        public async Task<IActionResult> Finished(int id, [FromBody] OrderFinishDto dto)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order != null)

            {
                order.IsClientFinish = dto.IsFinish;

                if (order.IsDriverFinish && order.IsClientFinish)
                {
                    order.IsFinish = true;
                }

                await _orderService.UpdateAsync(order);
                return Ok(new { Message = "Order Updated Successfully" });
            }

            return NotFound(new { message = "No order found with this id" });
        }

        [Authorize(Roles = "Client")]
        [HttpPut("updatedOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderExtensionDto dto)
        {
            var order = await _orderService.GetByIdAsync(id); 
            if (order != null)
            {
                order.Km = dto.Km;
                order.CarType = dto.CarType;
                order.CurrentLocation = dto.CurrentLocation;
                order.Destination = dto.Destination;
                order.ImagePath = dto.ImagePath;
                order.Message = dto.Message;
                order.OrderDate = dto.OrderDate;
                order.RoadPrice = dto.RoadPrice;
                order.CarPrice = dto.CarPrice;
                order.TotalPrice = dto.TotalPrice;
                order.UserId = dto.UserId;

                await _orderService.UpdateAsync(order);
                return Ok(new { Message = "Order Updated Successfully" });
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
