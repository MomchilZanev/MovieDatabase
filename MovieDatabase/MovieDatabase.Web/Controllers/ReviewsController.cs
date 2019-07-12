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
            bool isValidId = reviewService.IsValidMovieOrSeasonId(id);

            if (isValidId)
            {
                var allReviews = reviewService.GetAllMovieOrSeasonReviews(id);

                return View(allReviews);
            }

            return Redirect("/Home/Index");
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isValidId = reviewService.IsValidMovieOrSeasonId(id);

            if (isValidId)
            {
                var exists = reviewService.ReviewExists(userId, id);

                if (exists)
                {
                    var reviewInputModel = reviewService.GetUserReview(userId, id);

                    return View(reviewInputModel);
                }
            }

            return View();
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

            bool isValidId = reviewService.IsValidMovieOrSeasonId(input.Id);

            if (isValidId)
            {
                var exists = reviewService.ReviewExists(userId, input.Id);

                if (exists)
                {
                    var controller = reviewService.UpdateUserReview(userId, input.Id, input.Content, input.Rating);

                    return Redirect($"/{controller}/All/");
                }
                else
                {
                    var controller = reviewService.CreateUserReview(userId, input.Id, input.Content, input.Rating);

                    return Redirect($"/{controller}/All/");
                }
            }

            return Redirect("/");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isValidId = reviewService.IsValidMovieOrSeasonId(id);

            if (isValidId)
            {
                var exists = reviewService.ReviewExists(userId, id);

                if (exists)
                {
                    string controller = reviewService.DeleteUserReview(userId, id);

                    return Redirect($"/{controller}/All");
                }
            }

            return Redirect($"/Home/Index");
        }
    }
}