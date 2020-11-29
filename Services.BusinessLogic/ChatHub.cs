using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SocialNetwork.Services.BusinessLogic
{
    public class ChatHub : Hub
    {
        public async Task Send(string userFromId, string userToId)
        {
            await Clients.User(userToId).SendAsync("Send", userFromId);
        }
    }
}