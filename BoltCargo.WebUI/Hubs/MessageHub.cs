using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BoltCargo.WebUI.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MessageHub(UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Ping()
        {
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            if (userName != null)
            {
                var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName);
                if (currentUser != null && !currentUser.IsOnline)
                {
                    currentUser.IsOnline = true;
                    await _customIdentityUserService.UpdateAsync(currentUser);
                    await Clients.Others.SendAsync("ReceiveUserConnected", currentUser.Id, true);
                }
            }
        }
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            if (userName != null)
            {
                var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName!);

                if (currentUser != null)
                {
                    currentUser.IsOnline = true;

                    await _customIdentityUserService.UpdateAsync(currentUser);

                    await Clients.Others.SendAsync("ReceiveUserConnected", currentUser.Id, true);
                }
            }


            await base.OnConnectedAsync();

        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            if (userName != null)
            {

                var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName!);

                if (currentUser != null)
                {
                    currentUser.IsOnline = false;
                    await _customIdentityUserService.UpdateAsync(currentUser);

                    await Clients.Others.SendAsync("ReceiveUserDisconnected", currentUser.Id, false);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task AllUsers()
        {
            await Clients.Others.SendAsync("ReceiveUsers");
        }
        public async Task AllFeedBacks()
        {
            await Clients.Others.SendAsync("ReceiveFeedBacks");
        }

    }
}
