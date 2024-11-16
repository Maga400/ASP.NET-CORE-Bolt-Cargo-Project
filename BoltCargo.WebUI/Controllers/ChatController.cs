using BoltCargo.Business.Services.Abstracts;
using BoltCargo.DataAccess.Data;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
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
    public class ChatController : ControllerBase
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IRelationShipService _relationShipService;
        private readonly IRelationShipRequestService _relationShipRequestService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly CargoDbContext _context;

        public ChatController(UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IRelationShipService relationShipService, IRelationShipRequestService relationShipRequestService, IChatService chatService, IMessageService messageService, CargoDbContext context)
        {
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _relationShipService = relationShipService;
            _relationShipRequestService = relationShipRequestService;
            _chatService = chatService;
            _messageService = messageService;
            _context = context;
        }

        [HttpGet("Chats")]
        public async Task<IActionResult> GoChat(string id = "")
        {
            if (id == "")
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _customIdentityUserService.GetByUsernameAsync(userName);

                //var user = await _userManager.GetUserAsync(HttpContext.User);
                //var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == id || c.ReceiverId == user.Id && c.SenderId == id);
                var chats = _context.Chats.Include(nameof(Chat.Receiver)).Where(c => c.SenderId == user.Id || c.ReceiverId == user.Id);
                var chatBlocks = from c in chats
                                 let receiver = (user.Id != c.ReceiverId) ? c.Receiver : _context.Users.FirstOrDefault(u => u.Id == c.SenderId)
                                 select new Chat
                                 {
                                     Messages = c.Messages,
                                     Id = c.Id,
                                     SenderId = c.SenderId,
                                     Receiver = receiver,
                                     ReceiverId = receiver.Id,

                                 };
                var result = chatBlocks.ToList().Where(c => c.ReceiverId != user.Id);

                var model = new GoChatDto
                {
                    CurrentUserId = "",
                    CurrentReceiver = "",
                    CurrentReceiverImage = "",
                    CurrentChat = null,
                    Chats = result.Count() == 0 ? chatBlocks : result,
                    CurrentUserName = "",
                };

                return Ok(new { Chats = model });
            }
            else
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _customIdentityUserService.GetByUsernameAsync(userName);
                //var user = await _userManager.GetUserAsync(HttpContext.User);
                var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == id || c.ReceiverId == user.Id && c.SenderId == id);
                var chats = _context.Chats.Include(nameof(Chat.Receiver)).Where(c => c.SenderId == user.Id || c.ReceiverId == user.Id);
                var chatBlocks = from c in chats
                                 let receiver = (user.Id != c.ReceiverId) ? c.Receiver : _context.Users.FirstOrDefault(u => u.Id == c.SenderId)
                                 select new Chat
                                 {
                                     Messages = c.Messages,
                                     Id = c.Id,
                                     SenderId = c.SenderId,
                                     Receiver = receiver,
                                     ReceiverId = receiver.Id,

                                 };
                var r = await chatBlocks.ToListAsync();
                var result = r.Where(c => c.ReceiverId != user.Id);

                if (chat == null)
                {
                    chat = new Chat
                    {
                        ReceiverId = id,
                        SenderId = user.Id,
                        Messages = new List<Message>()
                    };

                    await _context.Chats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                }
                var model = new GoChatDto
                {
                    CurrentUserId = user.Id,
                    CurrentReceiver = id,
                    CurrentReceiverImage = _context.Users.FirstOrDefault(u => u.Id == id).ImagePath,
                    CurrentChat = chat,
                    Chats = result.Count() == 0 ? chatBlocks : result,
                    CurrentUserName = _context.Users.FirstOrDefault(u => u.Id == id).UserName,
                };

                return Ok(new { Chats = model });

            }
        }

        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage(AddMessageDto model)
        {
            try
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _customIdentityUserService.GetByUsernameAsync(userName);

                //var user = await _userManager.GetUserAsync(HttpContext.User);
                var chats = await _chatService.GetAllAsync();
                var chat = chats.FirstOrDefault(c => c.SenderId == model.SenderId && c.ReceiverId == model.ReceiverId || c.SenderId == model.ReceiverId && c.ReceiverId == model.SenderId);
                if (chat != null)
                {
                    var message = new Message
                    {
                        ChatId = chat.Id,
                        Content = model.Content,
                        DateTime = DateTime.Now,
                        IsImage = false,
                        HasSeen = false,
                        SenderId = user.Id,
                        ReceiverId = user.Id != model.ReceiverId ? model.ReceiverId : model.SenderId,
                    };
                    await _messageService.AddAsync(message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { Message = "Message Added Successfully" });
        }

        [HttpGet("AllMessages")]
        public async Task<IActionResult> GetAllMessages(string receiverId, string senderId)
        {
            var chats = await _chatService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.SenderId == senderId && c.ReceiverId == receiverId || c.SenderId == receiverId && c.ReceiverId == senderId);
            if (chat != null)
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _customIdentityUserService.GetByUsernameAsync(userName);

                //var user = await _userManager.GetUserAsync(HttpContext.User);
                return Ok(new { messages = chat.Messages, CurrentUserId = user.Id });
            }

            return NotFound(new { Message = "No found chat and chats messages!" });

        }

    }
}
