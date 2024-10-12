using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IMapper _mapper;

        public UserController(ICustomIdentityUserService customIdentityUserService, IMapper mapper)
        {
            _customIdentityUserService = customIdentityUserService;
            _mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<UserDto>> Get()
        {
            var users = await _customIdentityUserService.GetAllAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return usersDto;
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

        // POST api/<UserController>
        [HttpPost]
        [ValidateAntiForgeryToken]

        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
