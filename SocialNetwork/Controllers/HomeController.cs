using Microsoft.AspNetCore.Mvc;
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
