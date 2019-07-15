using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;

namespace MovieDatabase.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        public IActionResult All(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);

            if (idIsValidMovieOrSeasonId)
            {
                var itemType = reviewService.IsIdMovieOrSeasonId(id);

                switch (itemType)
                {
                    case "Movie":
                        var movieReviews = reviewService.GetAllMovieReviews(id);
                        return View(movieReviews);
                    case "Season":
                        var seasonReviews = reviewService.GetAllSeasonReviews(id);
                        return View(seasonReviews);
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect("/Home/Error");
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);

            if (idIsValidMovieOrSeasonId)
            {
                return View();
            }

            return Redirect("/Home/Error");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateReviewInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(input.Id);

            if (idIsValidMovieOrSeasonId)
            {
                var itemType = reviewService.IsIdMovieOrSeasonId(input.Id);

                switch (itemType)
                {
                    case "Movie":
                        if (!reviewService.CreateMovieReview(userId, input))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect($"/Movies/Details/{input.Id}");
                    case "Season":
                        if (!reviewService.CreateSeasonReview(userId, input))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect($"/TVShows/SeasonDetails/{input.Id}");
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect("/Home/Error");
        }

        [Authorize]
        public IActionResult Update(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);
            bool reviewExists = reviewService.ReviewExists(userId, id);

            if (idIsValidMovieOrSeasonId && reviewExists)
            {
                var itemType = reviewService.IsIdMovieOrSeasonId(id);

                switch (itemType)
                {
                    case "Movie":
                        var movieReview = reviewService.GetMovieReview(userId, id);

                        return View(movieReview);
                    case "Season":
                        var seasonReview = reviewService.GetSeasonReview(userId, id);

                        return View(seasonReview);
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect("/Home/Error");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(CreateReviewInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(input.Id);
            bool reviewExists = reviewService.ReviewExists(userId, input.Id);

            if (idIsValidMovieOrSeasonId && reviewExists)
            {
                var itemType = reviewService.IsIdMovieOrSeasonId(input.Id);

                switch (itemType)
                {
                    case "Movie":
                        if (!reviewService.UpdateMovieReview(userId, input))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect($"/Movies/Details/{input.Id}");
                    case "Season":
                        if (!reviewService.UpdateSeasonReview(userId, input))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect($"/TVShows/SeasonDetails/{input.Id}");
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect("/Home/Error");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(id);

            if (idIsValidMovieOrSeasonId)
            {
                var itemType = reviewService.IsIdMovieOrSeasonId(id);

                switch (itemType)
                {
                    case "Movie":
                        if (!reviewService.DeleteMovieReview(userId, id))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect($"/Movies/Details/{id}");
                    case "Season":
                        if (!reviewService.DeleteSeasonReview(userId, id))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect($"/TVShows/SeasonDetails/{id}");
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect($"/Home/Error");
        }
    }
}