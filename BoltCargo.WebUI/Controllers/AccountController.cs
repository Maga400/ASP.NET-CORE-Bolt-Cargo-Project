﻿using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Business.Services.Concretes;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
using BoltCargo.WebUI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoltCargo.WebUI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        public AccountController(UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager, RoleManager<CustomIdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ICustomIdentityUserService customIdentityUserService, IHubContext<MessageHub> hubContext, IHttpContextAccessor httpContextAccessor, ICardService cardService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _customIdentityUserService = customIdentityUserService;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
        }

        [HttpPost("existUser")]
        public async Task<IActionResult> ExistUser([FromQuery] string name)
        {
            var existingUser = await _userManager.FindByNameAsync(name);
            if (existingUser != null)
            {
                return BadRequest(new { Status = "Name Error", Message = "A user with this username already exists!" });
            }

            return Ok(new { Status = "Success" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {

            var user = new CustomIdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                ImagePath = dto.ImagePath,
                CarType = dto.CarType,
                PhoneNumber = dto.PhoneNumber,
                Name = dto.Name,
                Surname = dto.Surname,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _roleManager.CreateAsync(new CustomIdentityRole { Name = dto.Role });
                }

                await _userManager.AddToRoleAsync(user, dto.Role);

                var hubContext = _hubContext.Clients.All.SendAsync("AllUsers");
                await hubContext;

                var card = new Card
                {
                    User = user,
                    UserId = user.Id,
                    BankName = dto.BankName,
                    CardNumber = dto.CardNumber,
                    Balance = 0
                };

                await _cardService.AddAsync(card);

                return Ok(new { Status = "Success", Message = "User created successfuly!" });
            }

            return BadRequest(new { Status = "Error", Message = "User creation failed!", Error = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(dto.Username);

                if (user.IsBan)
                {
                    return BadRequest(new { Message = "This user has been banned by Admin",Error = "BAN"});
                }

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
                 
                user.IsOnline = true;
                await _customIdentityUserService.UpdateAsync(user);
                var token = GetToken(authClaims);

                var identity = new ClaimsIdentity(authClaims, "Registration");
                var principal = new ClaimsPrincipal(identity);

                _httpContextAccessor.HttpContext!.User = principal;

                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo });

            }

            return Unauthorized();
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

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName);
            
            if (currentUser == null)
            {
                return NotFound(new { Message = "Current user not found" });
            }
             
            var userRole = await _userManager.GetRolesAsync(currentUser);
            var currentUserDto = _mapper.Map<UserDto>(currentUser);
            return Ok(new { user = currentUserDto, role = userRole });

        }
           
        [Authorize]
        [HttpGet("logout")] 
        public async Task<IActionResult> Logout()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName);

            if (currentUser != null)
            {
                currentUser.IsOnline = false;
                await _customIdentityUserService.UpdateAsync(currentUser);
                return Ok(new { Message = "User logouted succesfully" });

            }

            return NotFound(new { Message = "User logouting failed" });
        }
    }
}
