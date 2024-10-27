using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoltCargo.WebUI.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RelationShipController : ControllerBase
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IRelationShipService _relationShipService;
        private readonly IRelationShipRequestService _relationShipRequestService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        public RelationShipController(UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IRelationShipService relationShipService, IRelationShipRequestService relationShipRequestService, IChatService chatService, IMessageService messageService)
        {
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _relationShipService = relationShipService;
            _relationShipRequestService = relationShipRequestService;
            _chatService = chatService;
            _messageService = messageService;
        }

        [HttpGet("allRelationShips")]
        public async Task<IActionResult> GetAllRelationShips()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _customIdentityUserService.GetByUsernameAsync(userName);

            //var user = await _userManager.GetUserAsync(HttpContext.User);
            var requests = await _relationShipRequestService.GetAllAsync();
            var datas = await _customIdentityUserService.GetAllAsync();
            var myRequests = requests.Where(r => r.SenderId == user.Id);
            var relationShips = await _relationShipService.GetAllAsync();
            var myRelationShips = relationShips.Where(f => f.OwnId == user.Id || f.YourRelationShipId == user.Id);

            var relationShipUsers = datas.Where(u => myRelationShips.Any(f => f.OwnId == u.Id || f.YourRelationShipId == u.Id) && u.Id != user.Id).ToList();

            return Ok(new { RelationShips = relationShipUsers });
        }


    }
}
