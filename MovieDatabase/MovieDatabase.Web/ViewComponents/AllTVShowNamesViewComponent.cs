using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllTVShowNamesViewComponent : ViewComponent
    {
        private readonly ITVShowService tvShowService;

        public AllTVShowNamesViewComponent(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genresAllViewModel = await tvShowService.GetAllTVShowNamesAsync();

            return View(genresAllViewModel);
        }
    }
}
