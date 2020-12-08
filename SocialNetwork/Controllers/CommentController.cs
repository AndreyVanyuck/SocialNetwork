using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.Domain.Core;
using SocialNetwork.Domain.Interfaces;

namespace SocialNetwork.Controllers
{
    public class CommentController: Controller
    {
        IUsersRepository _repository;
        User _user;

        public CommentController(IUsersRepository repository, 
                                IHttpContextAccessor httpContextAccessor,
                                UserManager<User> userManager)
        {
            _repository = repository;
            var id = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            _user = _repository.GetUserById(id); 
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.LoggedUser = _user;
        }

        [Route("addComment/{text}/{postId}")]
        public RedirectToActionResult Create(string text, int postId)
        {
            var comment = new Comment()
            {
                Owner = _user,
                Post = _repository.GetPostById(postId),
                Text = text,
                Date = DateTime.UtcNow //# TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")) 
            };

            _repository.Create(comment);
            _repository.Save();
            return RedirectToAction("CommentsList", new { postId });
        }

        [Route("removeComment/{commentId}")]
        public RedirectToActionResult Remove(int commentId)
        {
            var comment = _repository.GetCommentById(commentId);
            var postId = comment.Post.Id;
            _repository.Remove(comment);
            _repository.Save();
            return RedirectToAction("CommentsList", new { postId });
        }

        public PartialViewResult CommentsList(int postId)
        {
            Post post = _repository.GetPostById(postId);
            return PartialView(post.Comments);
        }
    }
}