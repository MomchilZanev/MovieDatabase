using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels;
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

        [Authorize]
        public IActionResult Create(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isValidId = reviewService.IsValidId(id);

            if (isValidId)
            {
                var exists = reviewService.Exists(userId, id);

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

            bool isValidId = reviewService.IsValidId(input.Id);

            var controller = "";
            if (isValidId)
            {
                var exists = reviewService.Exists(userId, input.Id);

                if (exists)
                {
                    controller = reviewService.UpdateUserReview(userId, input.Id, input.Content, input.Rating);

                    return Redirect($"/{controller}/All/");
                }
                else
                {
                    controller = reviewService.CreateUserReview(userId, input.Id, input.Content, input.Rating);

                    return Redirect($"/{controller}/All/");
                }
            }

            return Redirect("/");
        }
    }
}