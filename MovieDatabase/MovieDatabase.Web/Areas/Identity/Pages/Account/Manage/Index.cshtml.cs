using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.CustomValidationAttributes;
using MovieDatabase.Services.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<MovieDatabaseUser> userManager;
        private readonly SignInManager<MovieDatabaseUser> signInManager;
        private readonly IAvatarService avatarService;

        public IndexModel(
            UserManager<MovieDatabaseUser> userManager,
            SignInManager<MovieDatabaseUser> signInManager,
            IAvatarService avatarService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.avatarService = avatarService;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [MaxFileSize(ValidationConstants.userAvatarMaximumFileSizeInBytes, ErrorMessage = "Maximum allowed file size is {0} bytes")]
            public IFormFile Avatar { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var userName = await userManager.GetUserNameAsync(user);
            var email = await userManager.GetEmailAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
            };

            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                var userName = await userManager.GetUserNameAsync(user);
                Username = userName;
                return Page();
            }

            var email = await userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var avatar = Input.Avatar;
            if (avatar != null)
            {
                await avatarService.ChangeUserAvatarAsync(user.Id, avatar);
            }

            await signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
