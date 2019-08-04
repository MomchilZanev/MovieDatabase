using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.User;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class UsersController : AdministrationController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Promote()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Promote(PromoteUserInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await userService.PromoteUserAsync(input))
            {
                return Redirect(GlobalConstants.redirectError);
            }

            return Redirect(GlobalConstants.redirectHome);
        }
    }
}