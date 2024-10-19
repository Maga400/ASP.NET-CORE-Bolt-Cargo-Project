using Microsoft.AspNetCore.SignalR;

namespace BoltCargo.WebUI.Hubs
{
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveConnectInfo","User Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("ReceiveDisconnectInfo","User Disconnected");
        }

    }
}
