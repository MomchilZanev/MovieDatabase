using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Linq;
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

        public async Task<IViewComponentResult> InvokeAsync(string selectedArtistName = null)
        {
            var artistNamesAllViewModel = await artistService.GetAllArtistNamesAsync();

            if (!artistNamesAllViewModel.Any(artist => artist.FullName == selectedArtistName))
            {
                return View(artistNamesAllViewModel);
            }
            
            var selected = artistNamesAllViewModel.Single(x => x.FullName == selectedArtistName);
            var index = artistNamesAllViewModel.IndexOf(selected);
            var first = artistNamesAllViewModel[0];
            artistNamesAllViewModel[0] = selected;
            artistNamesAllViewModel[index] = first;

            return View(artistNamesAllViewModel);
        }
    }
}
