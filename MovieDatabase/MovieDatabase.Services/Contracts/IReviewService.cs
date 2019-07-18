using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        Task<bool> CreateMovieReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> CreateSeasonReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> UpdateMovieReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> UpdateSeasonReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> DeleteMovieReviewAsync(string userId, string movieId);

        Task<bool> DeleteSeasonReviewAsync(string userId, string seasonId);
    }
}
