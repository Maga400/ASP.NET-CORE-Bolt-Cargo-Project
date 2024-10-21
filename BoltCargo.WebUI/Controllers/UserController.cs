using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(ICustomIdentityUserService customIdentityUserService, IMapper mapper, UserManager<CustomIdentityUser> userManager)
        {
            _customIdentityUserService = customIdentityUserService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<UserDto>> Get()
        {
            var users = await _customIdentityUserService.GetAllAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return usersDto;
        }

        [HttpGet("AllDrivers")]
        public async Task<List<UserDto>> GetAllDrivers()
        {
            var drivers = await _userManager.GetUsersInRoleAsync("Driver");
            var orderedDrivers = drivers.OrderByDescending(u => u.IsOnline);
            var driverDtos = _mapper.Map<List<UserDto>>(orderedDrivers);
            return driverDtos;
        }

        [HttpGet("AllClients")]
        public async Task<List<UserDto>> GetAllClients()
        {
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            var orderedClients = clients.OrderByDescending(u => u.IsOnline);
            var clientDtos = _mapper.Map<List<UserDto>>(orderedClients);
            return clientDtos;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _customIdentityUserService.GetByIdAsync(id);
            if (user != null)
            {
                var userDto = _mapper.Map<UserDto>(user);
                return Ok(new { user = userDto });
            }
            return NotFound(new { message = "No user found with this id" });
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "No user found with this id!" });
            }

            var passwordHasher = new PasswordHasher<CustomIdentityUser>();
            user.UserName = dto.UserName;
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password); 
            user.Email = dto.Email;
            user.CarType = dto.CarType;
            user.ImagePath = dto.ImagePath;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User Updated Successfully " });
            }

            return BadRequest(new { Message = "Something went wrong! " });

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _customIdentityUserService.GetByIdAsync(id);
            if (user != null)
            {
                await _customIdentityUserService.DeleteAsync(user);
                return Ok(new { message = "User Deleted Successfully" });
            }

            return NotFound(new { message = "No user found with this id" });

        }


    }
}
