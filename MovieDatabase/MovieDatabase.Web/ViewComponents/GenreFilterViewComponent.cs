using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class GenreFilterViewComponent : ViewComponent
    {
        private readonly IGenreService genreService;

        public GenreFilterViewComponent(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genresAllViewModel = genreService.GetAllGenreNames();

            return View(genresAllViewModel);
        }
    }
}
