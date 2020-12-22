using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using SocialNetwork.ViewModels;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Core;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Services.BusinessLogic;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.Controllers
{
    [Obsolete]
    public class UserController : Controller
    {
        IUsersRepository _repository;
      //  IHostingEnvironment _environment;
        User _user;
        UserManager<User> _userManager;
        IMailService _mailService;

        public UserController(IUsersRepository repository,
                              //IHostingEnvironment environment,
                              IHttpContextAccessor httpContextAccessor,
                              UserManager<User> userManager,
                              IMailService mailService)
        {
            _repository = repository;
           // _environment = environment;
            var id = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            _user = _repository.GetUserById(id);
            _userManager = userManager;
            _mailService = mailService;

        }

        public async Task<ActionResult> Index(){
            _repository.GetUsersMainPageInfo(_user);
            var loggedRoles = (await _userManager.GetRolesAsync(_user)).ToList();
            ViewBag.LoggedUserRoles = loggedRoles;
            ViewBag.UserRoles = loggedRoles;
            if (_user.IsBlocked)
                return RedirectToAction("NoAccess", "Home");
            return View(_user);
        }

        public async Task<ActionResult> MainPage(string userId) {
            if (userId == _user.Id)
                return Redirect("~/User");
            User user = _repository.GetUserById(userId);
            _repository.GetUsersMainPageInfo(user);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var loggedRoles = (await _userManager.GetRolesAsync(_user)).ToList();
            ViewBag.LoggedUserRoles = loggedRoles;
            ViewBag.UserRoles = roles;
            if (_user.IsBlocked)
                return RedirectToAction("NoAccess", "Home");
            if (user.IsBlocked)
                return View("BlockedPage", user);
        
            return View("Index", user);
        }

        public ViewResult Friends(string userId = null)
        {
            if (_user.IsBlocked)
                return View("NoAccess", "Home");
            User user = userId == null ? _user:_repository.GetUserById(userId);  
            var friends =_repository.GetUsersFriends(user);
            var followers = user.Followers;
            var followings = user.Following;
            foreach (var friend in friends)
                _repository.GetUsersMainPhoto(friend);
            foreach (var follower in followers)
                _repository.GetUsersMainPhoto(follower);
            foreach (var following in followings)
                _repository.GetUsersMainPhoto(following);
            FriendsRequestsViewModel friendsVM = new FriendsRequestsViewModel
            {
                Friends = user.Friends,
                Followers = user.Followers,
                Following = user.Following
            };

            if (user == _user)
                friendsVM.Requests = user.IncomingFrienshipRequests.Where(r => r.Status == FriendshipStatus.Waiting)
                                                     .Select(r => r.RequestFrom).ToList();
            foreach (var r in friendsVM.Requests)
                _repository.GetUsersMainPhoto(r);
            return View(friendsVM);
        }

        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Block(string userId)
        {
            User user = _repository.GetUserById(userId);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin"))
                return await MainPage(userId);
            user.IsBlocked = true;
            _repository.Update(user);
            _repository.Save();
            await _mailService.SendEmailAsync(user.Email, "Support", String.Format("Hello {0} {1}, your account was blocked!", user.Name, user.Surname));
            return await MainPage(userId);
        }

        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Unblock(string userId)
        {
            User user = _repository.GetUserById(userId);
            user.IsBlocked = false;
            _repository.Update(user);
            _repository.Save();
            await _mailService.SendEmailAsync(user.Email, "Support", String.Format("Hello {0} {1}, your account was unblocked!", user.Name, user.Surname));
            return await MainPage(userId);
        }


        [HttpGet]
        public ViewResult Update()
        {
            return View(new UserInfoViewModel(_user));
        }

        public User ChangeInformation(User user, UserInfoViewModel info)
        {
            user.Name = info.Name;
            user.Surname = info.Surname;
            user.BirthDay = info.BirthDay;
            user.Email = info.Email;
            user.PhoneNumber = info.PhoneNumber;
            user.Country = info.Country;
            user.City = info.City;
            user.Address = info.Address;
            user.School = info.School;
            user.University = info.University;
            user.JobPlace = info.JobPlace;
            user.JobPosition = info.JobPosition;
            user.Gender = info.Gender;
            

            return user;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Update(UserInfoViewModel info)
        {
            if (info.Avatar != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(info.Avatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)info.Avatar.Length);
                }
                _user.Avatar = imageData;
            }
            _user = ChangeInformation(_user, info);
            _repository.Update(_user);
            _repository.Save();
            return RedirectToAction("Index");
        }

 

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.LoggedUser = _user;
        }
    }
}