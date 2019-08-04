using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private const string redirectMovieDetails = "/Movies/Details/";
        private const string redirectSeasonDetails = "/TVShows/SeasonDetails/";

        private readonly IReviewService reviewService;
        private readonly IUserService userService;

        public ReviewsController(IReviewService reviewService, IUserService userService)
        {
            this.reviewService = reviewService;
            this.userService = userService;
        }

        public async Task<IActionResult> All(string id)
        {
            bool idIsValidMovieOrSeasonId = await reviewService.IsValidMovieOrSeasonIdAsync(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(GlobalConstants.redirectError);
            }

            var itemType = await reviewService.IsIdMovieOrSeasonIdAsync(id);
            if (itemType == GlobalConstants.Movie)
            {
                var movieReviews = await reviewService.GetAllMovieReviewsAsync(id);

                return View(movieReviews);
            }
            else if (itemType == GlobalConstants.Season)
            {
                var seasonReviews = await reviewService.GetAllSeasonReviewsAsync(id);

                return View(seasonReviews);
            }
            else
            { return Redirect(GlobalConstants.redirectError); }
        }

        [Authorize]
        public async Task<IActionResult> Create(string id)
        {
            bool idIsValidMovieOrSeasonId = await reviewService.IsValidMovieOrSeasonIdAsync(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(GlobalConstants.redirectError);
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            bool idIsValidMovieOrSeasonId = await reviewService.IsValidMovieOrSeasonIdAsync(input.Id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(GlobalConstants.redirectError);
            }

            string userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);

            var itemType = await reviewService.IsIdMovieOrSeasonIdAsync(input.Id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.CreateMovieReviewAsync(userId, input))
                {
                    return Redirect(GlobalConstants.redirectError);
                }

                return Redirect(redirectMovieDetails + input.Id);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.CreateSeasonReviewAsync(userId, input))
                {
                    return Redirect(GlobalConstants.redirectError);
                }

                return Redirect(redirectSeasonDetails + input.Id);
            }
            else
            { return Redirect(GlobalConstants.redirectError); }
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            string userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);

            bool idIsValidMovieOrSeasonId = await reviewService.IsValidMovieOrSeasonIdAsync(id);
            bool reviewExists = await reviewService.ReviewExistsAsync(userId, id);
            if (!(idIsValidMovieOrSeasonId && reviewExists))
            {
                return Redirect(GlobalConstants.redirectError);
            }

            var itemType = await reviewService.IsIdMovieOrSeasonIdAsync(id);
            if (itemType == GlobalConstants.Movie)
            {
                var movieReview = await reviewService.GetMovieReviewAsync(userId, id);

                return View(movieReview);
            }
            else if (itemType == GlobalConstants.Season)
            {
                var seasonReview = await reviewService.GetSeasonReviewAsync(userId, id);

                return View(seasonReview);
            }
            else
            { return Redirect(GlobalConstants.redirectError); }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(CreateReviewInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            string userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);

            bool idIsValidMovieOrSeasonId = await reviewService.IsValidMovieOrSeasonIdAsync(input.Id);
            bool reviewExists = await reviewService.ReviewExistsAsync(userId, input.Id);
            if (!(idIsValidMovieOrSeasonId && reviewExists))
            {
                return Redirect(GlobalConstants.redirectError);
            }

            var itemType = await reviewService.IsIdMovieOrSeasonIdAsync(input.Id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.UpdateMovieReviewAsync(userId, input))
                {
                    return Redirect(GlobalConstants.redirectError);
                }

                return Redirect(redirectMovieDetails + input.Id);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.UpdateSeasonReviewAsync(userId, input))
                {
                    return Redirect(GlobalConstants.redirectError);
                }

                return Redirect(redirectSeasonDetails +  input.Id);
            }
            else
            { return Redirect(GlobalConstants.redirectError); }
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            bool idIsValidMovieOrSeasonId = await reviewService.IsValidMovieOrSeasonIdAsync(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(GlobalConstants.redirectError);
            }

            string userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);

            var itemType = await reviewService.IsIdMovieOrSeasonIdAsync(id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.DeleteMovieReviewAsync(userId, id))
                {
                    return Redirect(GlobalConstants.redirectError);
                }

                return Redirect(redirectMovieDetails + id);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.DeleteSeasonReviewAsync(userId, id))
                {
                    return Redirect(GlobalConstants.redirectError);
                }

                return Redirect(redirectSeasonDetails + id);
            }
            else
            { return Redirect(GlobalConstants.redirectError); }
        }
    }
}