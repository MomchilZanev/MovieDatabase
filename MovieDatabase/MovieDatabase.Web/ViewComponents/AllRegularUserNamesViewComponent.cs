using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllRegularUserNamesViewComponent : ViewComponent
    {
        private readonly IUserService userService;

        public AllRegularUserNamesViewComponent(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userNamesAllViewModel = await userService.GetAllRegularUserNamesAsync();

            return View(userNamesAllViewModel);
        }
    }
}
