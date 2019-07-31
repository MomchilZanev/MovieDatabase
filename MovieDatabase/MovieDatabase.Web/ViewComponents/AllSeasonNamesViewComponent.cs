using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllSeasonNamesViewComponent : ViewComponent
    {
        private readonly ITVShowService tvShowService;

        public AllSeasonNamesViewComponent(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var seasonsAllViewModel = await tvShowService.GetAllSeasonIdsSeasonNumbersAndTVShowNamesAsync();

            return View(seasonsAllViewModel);
        }
    }
}
