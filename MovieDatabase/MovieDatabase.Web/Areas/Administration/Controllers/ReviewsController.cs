using MovieDatabase.Services.Contracts;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class ReviewsController : AdministrationController
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }
    }
}