using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class FriendshipController : Controller
    {
        IUsersRepository _repository;
        User _user;

        public FriendshipController(IUsersRepository repository, 
                                    IHttpContextAccessor httpContextAccessor,
                                    UserManager<User> userManager)
        {
            _repository = repository;
            var id = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            _user = _repository.GetUserById(id);
        }

        public RedirectToActionResult AddToFriends(string userId)
        {
            var user = _repository.GetUserById(userId);
            var request = new Friendship()
            {
                RequestFrom = _user,
                RequestTo = user,
                Status = FriendshipStatus.Waiting
            };

            _repository.Create(request);
            _repository.Save();
            return RedirectToAction("MainPage", "User", new { userId });
        }

        public RedirectToActionResult ConfirmRequest(string userId)
        {
            var request = GetRequest(userId);
            request.Status = FriendshipStatus.Confirmed;
            _repository.Update(request);
            _repository.Save();
            return RedirectToAction("MainPage", "User", new { userId });
        }

        public RedirectToActionResult CancelRequest(string userId)
        {
            var request = GetRequest(userId);
            _repository.Remove(request);
            _repository.Save();
            return RedirectToAction("MainPage", "User", new { userId });
        }

        public RedirectToActionResult Remove(string userId)
        {
            var request = GetRequestToOwner(userId);
            var user = _repository.GetUserById(userId);

            if (request == null)
            {
                CancelRequest(userId);
                request = new Friendship
                {
                    RequestFrom = user,
                    RequestTo = _user,
                    Status = FriendshipStatus.Rejected
                };
                _repository.Create(request);
                _repository.Save();
                return RedirectToAction("MainPage", "User", new { userId });
            }
            request.Status = FriendshipStatus.Rejected;
            _repository.Update(request);
            _repository.Save();
            return RedirectToAction("MainPage", "User", new { userId });
        }

        [NonAction]
        private Friendship GetRequestToOwner(string userId)
        {
            var user = _repository.GetUserById(userId);
            return _repository.Friendships.SingleOrDefault(f =>
                    f.RequestFrom == user && f.RequestTo == _user);
        }


        [NonAction]
        private Friendship GetRequest(string userId)
        {
            var user = _repository.GetUserById(userId);
            return _repository.Friendships.Single(f =>
                    (f.RequestFrom == _user && f.RequestTo == user) ||
                    (f.RequestFrom == user && f.RequestTo == _user));
        }

        public RedirectToActionResult ConfirmRequestFromFriendsView(string userId)
        {
            var request = GetRequest(userId);
            request.Status = FriendshipStatus.Confirmed;
            _repository.Update(request);
            _repository.Save();
            return RedirectToAction("Friends", "User");
        }

        public RedirectToActionResult CancelRequestFromFriendsView(string userId)
        {
            var request = GetRequest(userId);
            request.Status = FriendshipStatus.Rejected;
            _repository.Update(request);
            _repository.Save();
            return RedirectToAction("Friends", "User");
        }


    }
}