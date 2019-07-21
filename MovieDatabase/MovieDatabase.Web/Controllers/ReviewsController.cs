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
        private const string redirectError = "/Home/Error";
        private const string redirectMovieDetails = "/Movies/Details/";
        private const string redirectSeasonDetails = "/TVShows/SeasonDetails/";

        private readonly IReviewService reviewService;
        private readonly IUserService userService;

        public ReviewsController(IReviewService reviewService, IUserService userService)
        {
            this.reviewService = reviewService;
            this.userService = userService;
        }

        public IActionResult All(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(redirectError);
            }

            var itemType = reviewService.IsIdMovieOrSeasonId(id);
            if (itemType == GlobalConstants.Movie)
            {
                var movieReviews = reviewService.GetAllMovieReviews(id);

                return View(movieReviews);
            }
            else if (itemType == GlobalConstants.Season)
            {
                var seasonReviews = reviewService.GetAllSeasonReviews(id);

                return View(seasonReviews);
            }
            else
            { return Redirect(redirectError); }
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(redirectError);
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

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(input.Id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(redirectError);
            }

            string userId = userService.GetUserIdFromUserName(User.Identity.Name);

            var itemType = reviewService.IsIdMovieOrSeasonId(input.Id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.CreateMovieReviewAsync(userId, input))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectMovieDetails + input.Id);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.CreateSeasonReviewAsync(userId, input))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectSeasonDetails + input.Id);
            }
            else
            { return Redirect(redirectError); }
        }

        [Authorize]
        public IActionResult Update(string id)
        {
            string userId = userService.GetUserIdFromUserName(User.Identity.Name);

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            bool reviewExists = reviewService.ReviewExists(userId, id);
            if (!(idIsValidMovieOrSeasonId && reviewExists))
            {
                return Redirect(redirectError);
            }

            var itemType = reviewService.IsIdMovieOrSeasonId(id);
            if (itemType == GlobalConstants.Movie)
            {
                var movieReview = reviewService.GetMovieReview(userId, id);

                return View(movieReview);
            }
            else if (itemType == GlobalConstants.Season)
            {
                var seasonReview = reviewService.GetSeasonReview(userId, id);

                return View(seasonReview);
            }
            else
            { return Redirect(redirectError); }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(CreateReviewInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            string userId = userService.GetUserIdFromUserName(User.Identity.Name);

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(input.Id);
            bool reviewExists = reviewService.ReviewExists(userId, input.Id);
            if (!(idIsValidMovieOrSeasonId && reviewExists))
            {
                return Redirect(redirectError);
            }

            var itemType = reviewService.IsIdMovieOrSeasonId(input.Id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.UpdateMovieReviewAsync(userId, input))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectMovieDetails + input.Id);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.UpdateSeasonReviewAsync(userId, input))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectSeasonDetails +  input.Id);
            }
            else
            { return Redirect(redirectError); }
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(redirectError);
            }

            string userId = userService.GetUserIdFromUserName(User.Identity.Name);

            var itemType = reviewService.IsIdMovieOrSeasonId(id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.DeleteMovieReviewAsync(userId, id))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectMovieDetails + id);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.DeleteSeasonReviewAsync(userId, id))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectSeasonDetails + id);
            }
            else
            { return Redirect(redirectError); }
        }
    }
}