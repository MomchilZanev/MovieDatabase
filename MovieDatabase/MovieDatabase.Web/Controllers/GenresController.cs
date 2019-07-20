using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class GenresController : Controller
    {
        private const string redirectHome = "/Home/Index";

        private readonly IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [Authorize(Roles = GlobalConstants.adminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.adminRoleName)]
        public async Task<IActionResult> Create(CreateGenreInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await genreService.CreateGenreAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectHome);
        }
    }
}