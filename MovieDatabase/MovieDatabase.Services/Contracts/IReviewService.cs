using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IReviewService
    {
        Task<bool> IsValidMovieOrSeasonIdAsync(string itemId);

        Task<string> IsIdMovieOrSeasonIdAsync(string id);

        Task<bool> ReviewExistsAsync(string userId, string itemId);

        Task<List<ReviewAllViewModel>> GetAllMovieReviewsAsync(string movieId);

        Task<List<ReviewAllViewModel>> GetAllSeasonReviewsAsync(string seasonId);

        Task<CreateReviewInputModel> GetMovieReviewAsync(string userId, string movieId);

        Task<CreateReviewInputModel> GetSeasonReviewAsync(string userId, string seasonId);

        Task<bool> CreateMovieReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> CreateSeasonReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> UpdateMovieReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> UpdateSeasonReviewAsync(string userId, CreateReviewInputModel input);

        Task<bool> DeleteMovieReviewAsync(string userId, string movieId);

        Task<bool> DeleteSeasonReviewAsync(string userId, string seasonId);
    }
}
