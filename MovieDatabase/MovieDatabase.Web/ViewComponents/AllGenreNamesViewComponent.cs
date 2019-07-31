using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllGenreNamesViewComponent : ViewComponent
    {
        private readonly IGenreService genreService;

        public AllGenreNamesViewComponent(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string selectedGenreName = null)
        {
            var genresAllViewModel = await genreService.GetAllGenreNamesAsync();

            if (!genresAllViewModel.Any(genre => genre.Name == selectedGenreName))
            {
                return View(genresAllViewModel);
            }

            var selected = genresAllViewModel.Single(x => x.Name == selectedGenreName);
            var index = genresAllViewModel.IndexOf(selected);
            var first = genresAllViewModel[0];
            genresAllViewModel[0] = selected;
            genresAllViewModel[index] = first;

            return View(genresAllViewModel);
        }
    }
}
