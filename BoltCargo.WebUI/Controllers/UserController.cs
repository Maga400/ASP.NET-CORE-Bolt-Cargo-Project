﻿using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoltCargo.WebUI.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(ICustomIdentityUserService customIdentityUserService, IMapper mapper, UserManager<CustomIdentityUser> userManager, IConfiguration configuration, SignInManager<CustomIdentityUser> signInManager)
        {
            _customIdentityUserService = customIdentityUserService;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
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

            if (dto.Password != null && dto.Password != "")
            {
                var passwordHasher = new PasswordHasher<CustomIdentityUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);
            }

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.CarType = dto.CarType;
            user.ImagePath = dto.ImagePath;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                var token = GetToken(authClaims);

                return Ok(new { Message = "User Updated Successfully ", Token = new JwtSecurityTokenHandler().WriteToken(token)});
            }

            return BadRequest(new { Message = "Something went wrong! " });

        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpPost("verifyPassword")]
        public async Task<IActionResult> VerifyPassword([FromBody] VerifyPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);

            if (user != null)
            {
                var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

                if (passwordValid)
                {
                    return Ok(new { Message = "This password is correct" });
                }

                return BadRequest( new {Message = "This password is incorrect" });
            }

            return NotFound(new { Message = "No user found with this id" });
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
