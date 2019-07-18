using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
