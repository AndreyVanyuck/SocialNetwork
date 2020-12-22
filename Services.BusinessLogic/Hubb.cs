/*using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Core;
using SocialNetwork.Domain.Interfaces;
using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


using System.Linq;


namespace SocialNetwork.Services.BusinessLogic
{
    public class Hubb
    {
        private readonly UserManager<User> _userManager;
        IUsersRepository _repository;


        public Hubb(UserManager<User> userManager, IUsersRepository userRepository)
        {
            _userManager = userManager;
            _repository = userRepository;
        }
        public async Task JoinPostGroup(string ArticleID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ArticleID);
        }

        public async Task SendMessage(string ArticleID, string Text)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            Comment comment = new Comment();
            comment.Profile = user;
            comment.DateTime = DateTime.UtcNow;
            comment.ArticleID = Int32.Parse(ArticleID);
            comment.Text = Text;
            await _commentRepository.Create(comment);
            await Clients.Group(ArticleID).SendAsync("ReceiveMessage", user.UserName, comment.Text, comment.DateTime);
        }


    }
}
*/