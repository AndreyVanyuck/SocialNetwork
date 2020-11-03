using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        IUsersRepository _repository;

        public HomeController(IUsersRepository repository)
        {
            _repository = repository;
        }


        public RedirectToActionResult Index()
        {
            return  RedirectToAction("Index", "User");
        }

    }
}

