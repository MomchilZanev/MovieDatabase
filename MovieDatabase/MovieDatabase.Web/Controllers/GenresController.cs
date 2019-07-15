using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Services.Contracts;

namespace MovieDatabase.Web.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateGenreInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!genreService.CreateGenre(input))
            {
                return View(input);
            }

            return Redirect("/Home/Index");
        }
    }
}