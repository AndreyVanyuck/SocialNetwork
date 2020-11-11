using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using System;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        IUsersRepository _repository;

        public HomeController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "User");
            return View();
        }

        public ActionResult NoAccess()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

    }
}
