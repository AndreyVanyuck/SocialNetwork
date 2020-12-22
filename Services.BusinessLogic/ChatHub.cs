using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Domain.Core;
using SocialNetwork.Domain.Interfaces;

namespace SocialNetwork.Services.BusinessLogic
{
    public class ChatHub : Hub
    {
/*        public async Task Send(string userFromId, string userToId)
        {
            await Clients.User(userToId).SendAsync("Send", userFromId);
        }
*/
        private readonly UserManager<User> _userManager;
        IUsersRepository _repository;


        public ChatHub(UserManager<User> userManager, IUsersRepository userRepository)
        {
            _userManager = userManager;
            _repository = userRepository;
        }
        public async Task JoinPostGroup(string ArticleID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ArticleID);
        }

        public async Task SendMessage(string PostId, string Text)
        {
          

            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            var comment = new Comment()
            {
                Owner = user,
                Post = _repository.GetPostById(Int32.Parse(PostId)),
                Text = Text,
                Date = DateTime.UtcNow
            };

            _repository.Create(comment);
            _repository.Save();
            await Clients.Group(PostId).SendAsync("ReceiveMessage", user.UserName, comment.Text, comment.Date);
        }
    }
}