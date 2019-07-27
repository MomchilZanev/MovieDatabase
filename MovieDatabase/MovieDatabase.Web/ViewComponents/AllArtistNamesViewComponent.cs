using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllArtistNamesViewComponent : ViewComponent
    {
        private readonly IArtistService artistService;

        public AllArtistNamesViewComponent(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genresAllViewModel = await artistService.GetAllArtistNamesAsync();

            return View(genresAllViewModel);
        }
    }
}
