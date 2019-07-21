using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class GenresController : AdministrationController
    {
        private const string redirectHome = "/Home/Index";

        private readonly IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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