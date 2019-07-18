using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> All(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect("/Home/Error");
            }

            var itemType = reviewService.IsIdMovieOrSeasonId(id);
            if (itemType == "Movie")
            {
                var movieReviews = reviewService.GetAllMovieReviews(id);

                return View(movieReviews);
            }
            else if (itemType == "Season")
            {
                var seasonReviews = reviewService.GetAllSeasonReviews(id);

                return View(seasonReviews);
            }
            else
            { return Redirect("/Home/Error"); }
        }

        [Authorize]
        public async Task<IActionResult> Create(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect("/Home/Error");
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
                return Redirect("/Home/Error");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var itemType = reviewService.IsIdMovieOrSeasonId(input.Id);
            if (itemType == "Movie")
            {
                if (!await reviewService.CreateMovieReviewAsync(userId, input))
                {
                    return Redirect("/Home/Error");
                }

                return Redirect($"/Movies/Details/{input.Id}");
            }
            else if (itemType == "Season")
            {
                if (!await reviewService.CreateSeasonReviewAsync(userId, input))
                {
                    return Redirect("/Home/Error");
                }

                return Redirect($"/TVShows/SeasonDetails/{input.Id}");
            }
            else
            { return Redirect("/Home/Error"); }
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            bool reviewExists = reviewService.ReviewExists(userId, id);
            if (!(idIsValidMovieOrSeasonId && reviewExists))
            {
                return Redirect("/Home/Error");
            }

            var itemType = reviewService.IsIdMovieOrSeasonId(id);
            if (itemType == "Movie")
            {
                var movieReview = reviewService.GetMovieReview(userId, id);

                return View(movieReview);
            }
            else if (itemType == "Season")
            {
                var seasonReview = reviewService.GetSeasonReview(userId, id);

                return View(seasonReview);
            }
            else
            { return Redirect("/Home/Error"); }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(CreateReviewInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(input.Id);
            bool reviewExists = reviewService.ReviewExists(userId, input.Id);
            if (!(idIsValidMovieOrSeasonId && reviewExists))
            {
                return Redirect("/Home/Error");
            }

            var itemType = reviewService.IsIdMovieOrSeasonId(input.Id);
            if (itemType == "Movie")
            {
                if (!await reviewService.UpdateMovieReviewAsync(userId, input))
                {
                    return Redirect("/Home/Error");
                }

                return Redirect($"/Movies/Details/{input.Id}");
            }
            else if (itemType == "Season")
            {
                if (!await reviewService.UpdateSeasonReviewAsync(userId, input))
                {
                    return Redirect("/Home/Error");
                }

                return Redirect($"/TVShows/SeasonDetails/{input.Id}");
            }
            else
            { return Redirect("/Home/Error"); }
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect($"/Home/Error");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var itemType = reviewService.IsIdMovieOrSeasonId(id);
            if (itemType == "Movie")
            {
                if (!await reviewService.DeleteMovieReviewAsync(userId, id))
                {
                    return Redirect("/Home/Error");
                }

                return Redirect($"/Movies/Details/{id}");
            }
            else if (itemType == "Season")
            {
                if (!await reviewService.DeleteSeasonReviewAsync(userId, id))
                {
                    return Redirect("/Home/Error");
                }

                return Redirect($"/TVShows/SeasonDetails/{id}");
            }
            else
            { return Redirect("/Home/Error"); }
        }
    }
}