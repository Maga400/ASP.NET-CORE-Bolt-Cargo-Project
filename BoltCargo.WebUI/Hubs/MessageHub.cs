using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
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
        private readonly IMapper _mapper;
        public MessageHub(UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        //public async Task Ping()
        //{
        //    var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        //    if (userName != null)
        //    {
        //        var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName);
        //        if (currentUser != null && !currentUser.IsOnline)
        //        {
        //            currentUser.IsOnline = true;
        //            await _customIdentityUserService.UpdateAsync(currentUser);
        //            await Clients.Others.SendAsync("ReceiveUserConnected", currentUser.Id, true);
        //        }
        //    }
        //}
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
                    //await AllUsers();

                    await Clients.Others.SendAsync("ReceiveUserConnected");
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
                    await AllUsers();

                    await Clients.Others.SendAsync("ReceiveUserDisconnected");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task AllUsers()
        {
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            if (userName != null)
            {
                var currentUser = await _customIdentityUserService.GetByUsernameAsync(userName!);

                if (currentUser != null)
                {
                    var isClient = await _userManager.IsInRoleAsync(currentUser, "Client");
                    //var isDriver = await _userManager.IsInRoleAsync(currentUser, "Driver");

                    if (isClient)
                    {
                        var drivers = await _userManager.GetUsersInRoleAsync("Client");
                        var users = drivers.OrderByDescending(u => u.IsOnline);
                        var userDtos = _mapper.Map<List<UserDto>>(users);


                        await Clients.Others.SendAsync("ReceiveUsers", userDtos);

                    }

                    else
                    {
                        var clients = await _userManager.GetUsersInRoleAsync("Driver");
                        var users = clients.OrderByDescending(u => u.IsOnline);
                        var userDtos = _mapper.Map<List<UserDto>>(users);

                        await Clients.Others.SendAsync("ReceiveUsers", userDtos);

                    }
                    //await AllUsers();

                }
            }
            //await Clients.Others.SendAsync("ReceiveUsers");
        }
        public async Task AllFeedBacks()
        {
            await Clients.Others.SendAsync("ReceiveFeedBacks");
        }
        public async Task AllOrders()
        {
            await Clients.Others.SendAsync("ReceiveOrders");
        }
        public async Task AllAdminOrders()
        {
            await Clients.Others.SendAsync("ReceiveAdminOrders");
        }
        public async Task AllGivenOrders()
        {
            await Clients.Others.SendAsync("ReceiveGivenOrders");
        }
        public async Task AllClientOrders()     
        {
            await Clients.Others.SendAsync("ReceiveClientOrders");
        }
        public async Task AllDriverOrders()
        {
            await Clients.Others.SendAsync("ReceiveDriverOrders");
        }
        public async Task AllFinishOrders()
        {
            await Clients.Others.SendAsync("ReceiveFinishOrders");
        }
        public async Task SendUser()
        {
            await Clients.Others.SendAsync("ReceiveUser");
        }
        public async Task AllMessages(string receiverId,string senderId)
        {
            await Clients.Others.SendAsync("ReceiveMessages",receiverId,senderId);
        }
        public async Task ChangeUserProfile()
        {
            await Clients.Others.SendAsync("ReceiveUserProfile");
        }
    }
}