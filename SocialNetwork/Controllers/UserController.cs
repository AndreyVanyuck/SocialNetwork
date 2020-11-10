using SocialNetwork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using SocialNetwork.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Controllers 
{
    public class UserController : Controller
    {
        IUsersRepository _repository;
        IHostingEnvironment _environment;

        User _user;
        public UserController(IUsersRepository repository,
                              IHostingEnvironment environment,
                              IHttpContextAccessor httpContextAccessor,
                              UserManager<User> userManager)
        {
            _repository = repository;
            _environment = environment;
            var id = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            _user = _repository.GetUserById(id);
        }

        public ViewResult Index(){
            _repository.GetUsersMainPageInfo(_user);
            return View(_user);
        }

        public ActionResult MainPage(string userId) {
            if (userId == _user.Id)
                return Redirect("~/User");
            User user = _repository.GetUserById(userId);
            _repository.GetUsersMainPageInfo(user);
            return View("Index", user);
        }

/*        public ActionResult Posts(int userId)
        {
            User user = userId == 0 ? _user : _repository.GetUserById(userId);
            _repository.GetUsersPosts(user);
            return PartialView(user.WallPosts);
        }*/
        public ViewResult Friends(string userId = null)
        {
            User user = userId == null ? _user:_repository.GetUserById(userId);  
            var friends =_repository.GetUsersFriends(user);
            /*foreach(var friend in friends)
                 _repository.GetUsersMainPhoto(friend);*/
            FriendsRequestsViewModel friendsVM = new FriendsRequestsViewModel
            {
                Friends = user.Friends
            };

            if (user == _user)
                friendsVM.Requests = user.IncomingFrienshipRequests.Where(r => r.Status == FriendshipStatus.Waiting)
                                                     .Select(r => r.RequestFrom).ToList();
    /*        foreach (var r in friendsVM.Requests)
                _repository.GetUsersMainPhoto(r);*/
            return View(friendsVM);
        }

        [HttpGet]
        public ViewResult Update()
        {
            return View(new UserInfoViewModel(_user));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Update(UserInfoViewModel info)
        {
            if (info.Avatar != null)
            {
                await UpdateAvatar(info.Avatar);
            }
            _user.ChangeInformation(info);
            _repository.Update(_user);
            _repository.Save();
            return RedirectToAction("Index");
        }

        [NonAction]
        public async Task UpdateAvatar(IFormFile avatar)
        {
            string path = "/usersPhotos/avatar" + _user.Id + ".jpg";
            using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
            {
                await avatar.CopyToAsync(fileStream);
            }

            var prevPhoto = _repository.GetUsersMainPhoto(_user);
            if (prevPhoto != null)
                _repository.Remove(prevPhoto);

            Post mainPhoto = new Post { Owner = _user, Date = DateTime.Now, Type = PostType.MainPhoto };
            Photo photo = new Photo { Image = path, Post = mainPhoto };
            _repository.Create(mainPhoto);
            _repository.Create(photo);

            _repository.Update(_user);
            _repository.Save();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.LoggedUser = _user;
        }
    }
}