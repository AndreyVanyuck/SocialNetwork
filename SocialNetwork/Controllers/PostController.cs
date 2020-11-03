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

        public string CreatePost(string text, string[] photos = null)
        {
            var post = new Post()
            {
                Owner = _user,
                Date = DateTime.Now,
                Text = text,
                Type = PostType.Normal
            };

            foreach (var photo in photos)
            {
                _repository.Create(
                    new Photo
                    {
                        Image = photo,
                        Post = post,
                    });
            }
            _repository.Create(post);
            _repository.Save();
            return "Created";
        }


        public string CreatePhoto(string text, string photo, bool isMain)
        {

            var post = new Post()
            {
                Owner = _user,
                Date = DateTime.Now,
                Text = text,
            };

            post.Type = isMain ? PostType.MainPhoto : PostType.PhotoOnly;

            _repository.Create(
                new Photo
                {
                    Image = photo,
                    Post = post,
                });
           
            _repository.Create(post);
            return "Created";
        }

        public string Remove(Post post)
        {
            _repository.Remove(post);
            _repository.Save();
            return "Removed";
        }
    }
}