using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.Domain.Core;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.ViewModels;

namespace SocialNetwork.Controllers
{
    [Obsolete]
    public class PostController : Controller
    {
        IUsersRepository _repository;
        User _user;
        IHostingEnvironment _environment;

        const int pageSize = 5;

        public PostController(IUsersRepository repository,
                             IHttpContextAccessor httpContextAccessor,
                             IHostingEnvironment environment,
                             UserManager<User> userManager)
        {
            _repository = repository;
            _environment = environment;
            var id = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            _user = _repository.GetUserById(id);
        }

        public ActionResult Index(int postId)
        {
            var post = _repository.GetPostById(postId);
            return PartialView(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PostViewModel postVM)
        {
            if (postVM.Text == null)
                return RedirectToAction("PostsList");

            var post = new Post()
            {
                Owner = _user,
                Date = DateTime.UtcNow,
                Text = postVM.Text,
                Type = PostType.Normal
            };
            if (postVM.Photo != null)
                await AddPhotoAsync(postVM.Photo, post);

            _repository.Create(post);
            _repository.Save();
            return RedirectToAction("PostsList");

        }
        private async Task AddPhotoAsync(IFormFile photo, Post post)
        {
            string path = "/usersPostPhotos/post" + _user.Id + DateTime.Now.ToString("MMddyyyyHHmmss") + ".jpg";
            using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            Photo photoToSave = new Photo { Image = path, Post = post };
            _repository.Create(photoToSave);
        }



        [Route("removePost/{postId}")]
        public ActionResult Remove(int postId)
        {
            var post = _repository.GetPostById(postId);
            _repository.Remove(post);
            _repository.Save();
            return RedirectToAction("PostsList");
        }

        public PartialViewResult PostsList(string userId = null)
        {
            User user = userId == null ? _user : _repository.GetUserById(userId);
            _repository.GetUsersPosts(user);
            return PartialView(user.WallPosts);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.LoggedUser = _user;
        }
    }
}