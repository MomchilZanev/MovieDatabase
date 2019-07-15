using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IReviewService
    {
        bool IsValidMovieOrSeasonId(string itemId);

        string IsIdMovieOrSeasonId(string id);

        bool ReviewExists(string userId, string itemId);

        List<ReviewAllViewModel> GetAllMovieReviews(string movieId);

        List<ReviewAllViewModel> GetAllSeasonReviews(string seasonId);

        CreateReviewInputModel GetMovieReview(string userId, string movieId);

        CreateReviewInputModel GetSeasonReview(string userId, string seasonId);

        bool CreateMovieReview(string userId, CreateReviewInputModel input);

        bool CreateSeasonReview(string userId, CreateReviewInputModel input);

        bool UpdateMovieReview(string userId, CreateReviewInputModel input);

        bool UpdateSeasonReview(string userId, CreateReviewInputModel input);

        bool DeleteMovieReview(string userId, string movieId);

        bool DeleteSeasonReview(string userId, string seasonId);
    }
}
