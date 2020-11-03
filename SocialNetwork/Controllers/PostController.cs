using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class PostController : Controller
    {
        IUsersRepository _repository;
        User _user;

        public PostController(IUsersRepository repository)
        {
            _repository = repository;
            _user = ((List<User>)_repository.Users)[0];
        }

        public ActionResult Index(Post post)
        {
            return PartialView(post);
        }

        [Route("createPost/{text}")]
        public ActionResult Create(string text)
        {
            var post = new Post()
            {
                Owner = _user,
                Date = DateTime.Now,
                Text = text,
                Type = PostType.Normal
            };
            _repository.Create(post);
            _repository.Save();
            return RedirectToAction("Posts", "User");
        }


        [Route("removePost/{postId}")]
        public ActionResult Remove(int postId)
        {
            var post = _repository.GetPostById(postId);
            _repository.Remove(post);
            _repository.Save();
            return RedirectToAction("Posts", "User");
        }
    }
}