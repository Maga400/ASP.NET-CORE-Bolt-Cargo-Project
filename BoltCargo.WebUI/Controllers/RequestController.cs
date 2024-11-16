using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Business.Services.Concretes;
using BoltCargo.DataAccess.Data;
using BoltCargo.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoltCargo.WebUI.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IRelationShipRequestService _relationShipRequestService;
        private readonly IRelationShipService _relationShipService;
        private readonly IChatService _chatService;
        private readonly CargoDbContext _context;
        public RequestController(ICustomIdentityUserService customIdentityUserService, UserManager<CustomIdentityUser> userManager, IRelationShipRequestService relationShipRequestService, IRelationShipService relationShipService, IChatService chatService, CargoDbContext context)
        {
            _customIdentityUserService = customIdentityUserService;
            _userManager = userManager;
            _relationShipRequestService = relationShipRequestService;
            _relationShipService = relationShipService;
            _chatService = chatService;
            _context = context;
        }

        [HttpPost("sendRelationShip")]
        public async Task<IActionResult> SendRelationShip(string id)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var sender = await _customIdentityUserService.GetByUsernameAsync(userName);

            //var sender = await _userManager.GetUserAsync(HttpContext.User);

            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (receiverUser != null)
            {
                await _relationShipRequestService.AddAsync(new RelationShipRequest
                {
                    Content = $"{sender.UserName} sent relationship request at {DateTime.Now.ToLongDateString()}",
                    SenderId = sender.Id,
                    Sender = sender,
                    ReceiverId = id,
                    Status = "Request"
                });

                return Ok(new { Message = "RelationShip Request Sended Successfully" });

            }

            return BadRequest(new { Message = "RelationShip Request Failed" });
        }

        [HttpDelete("takeRequest")]
        public async Task<IActionResult> TakeRequest(string id)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var current = await _customIdentityUserService.GetByUsernameAsync(userName);

            //var current = await _userManager.GetUserAsync(HttpContext.User);
            var relationShipRequests = await _relationShipRequestService.GetAllAsync();
            var request = relationShipRequests.FirstOrDefault(r => r.SenderId == current.Id && r.ReceiverId == id);

            if (request == null) return NotFound(new { Message = "No found relation request" });

            await _relationShipRequestService.DeleteAsync(request);

            return Ok(new { Message = "Relationship Request Taken Successfully" });
        }

        [HttpDelete("toCloseRelation")]
        public async Task<IActionResult> ToCloseRelation(string id)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _customIdentityUserService.GetByUsernameAsync(userName);
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            var relations = await _relationShipService.GetAllAsync();
            var relation = relations.FirstOrDefault(r => r.YourRelationShipId == user.Id && r.OwnId == id || r.OwnId == user.Id && r.YourRelationShipId == id);
            var chats = await _chatService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.ReceiverId == relation.YourRelationShipId && c.SenderId == relation.OwnId || c.ReceiverId == relation.OwnId && c.SenderId == relation.YourRelationShipId);


            if (relation != null)
            {
                await _relationShipService.DeleteAsync(relation);
                await _chatService.DeleteAsync(chat);
                return Ok(new { Message = "Closed Relation Successfully" });
            }

            return NotFound(new { Message = "No found relation" });

        }

        [HttpGet("allRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var current = await _customIdentityUserService.GetByUsernameAsync(userName);

            //var current = await _userManager.GetUserAsync(HttpContext.User);
            var relationShipRequests = await _relationShipRequestService.GetAllAsync();
            var requests = relationShipRequests.Where(r => r.ReceiverId == current.Id);

            return Ok(new { Requests = requests });
        }

        [HttpGet("acceptRequest")]
        public async Task<IActionResult> AcceptRequest(string userId, string senderId)
        {
            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var sender = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == senderId);

            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _customIdentityUserService.GetByUsernameAsync(userName);

            //var user = await _userManager.GetUserAsync(HttpContext.User);
            var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == sender.Id || c.ReceiverId == user.Id && c.SenderId == sender.Id);

            if (receiverUser != null)
            {

                await _relationShipService.AddAsync(new RelationShip
                {
                    OwnId = sender.Id,
                    YourRelationShipId = receiverUser.Id,
                    RelationShipDate = DateTime.Now,
                });

                await _userManager.UpdateAsync(receiverUser);

                if (chat == null)
                {
                    chat = new Chat
                    {
                        ReceiverId = receiverUser.Id,
                        SenderId = senderId,
                        Messages = new List<Message>()
                    };

                    await _context.Chats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { Message = "Request Accepted Successfully" });
            }

            return BadRequest(new { Message = "Accept Request Failed!" });
        }

        [HttpGet("declineRequest")]
        public async Task<IActionResult> DeclineRequest(int id, string senderid)
        {
            try
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var current = await _customIdentityUserService.GetByUsernameAsync(userName);

                //var current = await _userManager.GetUserAsync(HttpContext.User);
                var relationShipRequests = await _relationShipRequestService.GetAllAsync();
                var request = relationShipRequests.FirstOrDefault(f => f.Id == id);
                await _relationShipRequestService.DeleteAsync(request);

                await _relationShipRequestService.AddAsync(new RelationShipRequest
                {
                    Content = $"{current.UserName} declined your relationShip request at {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString}",
                    SenderId = current.Id,
                    Sender = current,
                    ReceiverId = senderid,
                    Status = "Notification"
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteRequest/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            try
            {
                var relationShipRequests = await _relationShipRequestService.GetAllAsync();
                var request = relationShipRequests.FirstOrDefault();
                if (request == null) return NotFound(new { Message = "No found request with this id!" });

                await _relationShipRequestService.DeleteAsync(request);
                return Ok(new { Message = "Request Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
