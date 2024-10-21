using BoltCargo.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BoltCargo.WebUI.Hubs
{
    public class MessageHub : Hub
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        public MessageHub(UserManager<CustomIdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(Context.User);

            if (user != null)
            {
                user.IsOnline = true;
                await _userManager.UpdateAsync(user);

                await Clients.All.SendAsync("UserOnlineStatusChanged", user.Id, true);
            }

            await base.OnConnectedAsync();

        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await _userManager.GetUserAsync(Context.User);

            if (user != null)
            {
                user.IsOnline = false;
                await _userManager.UpdateAsync(user);

                await Clients.All.SendAsync("UserOnlineStatusChanged", user.Id, false);
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
