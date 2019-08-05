using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewComponents
{
    public class AllRegularUserNamesViewComponent : ViewComponent
    {
        private readonly UserManager<MovieDatabaseUser> userManager;
        private readonly IMapper mapper;

        public AllRegularUserNamesViewComponent(UserManager<MovieDatabaseUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userNamesAllViewModel = new List<UserNameViewModel>();

            if (User.IsInRole(GlobalConstants.adminRoleName))
            {
                var allRegularUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.userRoleName);

                userNamesAllViewModel = mapper.Map<IList<MovieDatabaseUser>, List<UserNameViewModel>>(allRegularUsers);

                return View(userNamesAllViewModel);
            }

            return View(userNamesAllViewModel);
        }
    }
}
