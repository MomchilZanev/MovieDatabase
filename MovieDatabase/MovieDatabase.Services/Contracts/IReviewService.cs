using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IReviewService
    {
        bool IsValidMovieOrSeasonId(string itemId);

        bool ReviewExists(string userId, string itemId);

        List<ReviewAllViewModel> GetAllMovieOrSeasonReviews(string itemId);

        CreateReviewInputModel GetUserReview(string userId, string itemId);

        string DeleteUserReview(string userId, string itemId);

        string CreateUserReview(string userId, string itemId, string content, int rating);

        string UpdateUserReview(string userId, string itemId, string content, int rating);
    }
}
