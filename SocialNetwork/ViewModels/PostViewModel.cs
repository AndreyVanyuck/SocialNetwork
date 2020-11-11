using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SocialNetwork.ViewModels
{
    public class PostViewModel
    {
        public IFormFile Photo { get; set; }
        public string Text { get; set; }
    }
}