using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Linq;
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

        public async Task<IViewComponentResult> InvokeAsync(string selectedTvShowName = null)
        {
            var tvShowNamesAllViewModel = await tvShowService.GetAllTVShowNamesAsync();

            if (!tvShowNamesAllViewModel.Any(genre => genre.Name == selectedTvShowName))
            {
                return View(tvShowNamesAllViewModel);
            }

            var selected = tvShowNamesAllViewModel.Single(x => x.Name == selectedTvShowName);
            var index = tvShowNamesAllViewModel.IndexOf(selected);
            var first = tvShowNamesAllViewModel[0];
            tvShowNamesAllViewModel[0] = selected;
            tvShowNamesAllViewModel[index] = first;

            return View(tvShowNamesAllViewModel);
        }
    }
}
