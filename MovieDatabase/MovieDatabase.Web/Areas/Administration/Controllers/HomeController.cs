using Microsoft.AspNetCore.Mvc;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class HomeController : AdministrationController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}