using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllMovieNamesViewComponent : ViewComponent
    {
        private readonly IMovieService movieService;

        public AllMovieNamesViewComponent(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genresAllViewModel = movieService.GetAllMovieNames();

            return View(genresAllViewModel);
        }
    }
}
