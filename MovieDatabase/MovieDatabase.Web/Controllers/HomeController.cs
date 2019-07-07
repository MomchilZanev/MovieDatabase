using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.ViewModels;
using MovieDatabase.Services.Contracts;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieDatabase.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAvatarService avatarService;

        public HomeController(IAvatarService avatarService)
        {
            this.avatarService = avatarService;
        }

        public IActionResult Index()
        {
            string avatarLink = "/user_avatars/no_avatar.jpg";
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                avatarLink = avatarService.GetUserAvatarLink(userId);
            }

            ViewData["UserAvatar"] = avatarLink;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
