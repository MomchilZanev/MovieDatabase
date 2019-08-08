using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MovieDatabaseUser> signInManager;
        private readonly UserManager<MovieDatabaseUser> userManager;
        private readonly ILogger<RegisterModel> logger;

        public RegisterModel(UserManager<MovieDatabaseUser> userManager, SignInManager<MovieDatabaseUser> signInManager, ILogger<RegisterModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(ValidationConstants.userNameMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.userNameMinimumLength)]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(ValidationConstants.userPasswordMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.userPasswordMinimumLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new MovieDatabaseUser { UserName = Input.UserName, Email = Input.Email, AvatarLink = GlobalConstants.noUserAvatar };

                var result = await userManager.CreateAsync(user, Input.Password);

                if (userManager.Users.Count() == 1)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.adminRoleName);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.userRoleName);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
