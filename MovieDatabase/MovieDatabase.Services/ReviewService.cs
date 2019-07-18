using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class ReviewService : IReviewService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public ReviewService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }        

        public bool IsValidMovieOrSeasonId(string itemId)
        {
            if (dbContext.Movies.Any(movie => movie.Id == itemId) || dbContext.Seasons.Any(t => t.Id == itemId))
            {
                return true;
            }

            return false;
        }

        public string IsIdMovieOrSeasonId(string id)
        {
            if (dbContext.Movies.Any(movie => movie.Id == id))
            {
                return "Movie";
            }
            else if (dbContext.Seasons.Any(season => season.Id == id))
            {
                return "Season";
            }

            return "Neither";
        }

        public bool ReviewExists(string userId, string itemId)
        {
            if (dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId) ||
                dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public List<ReviewAllViewModel> GetAllMovieReviews(string movieId)
        {
            var movieReviewsFromDb = dbContext.MovieReviews.Where(movieReview => movieReview.MovieId == movieId);

            var reviewsAllViewModel = movieReviewsFromDb
                    .Select(movieReview => new ReviewAllViewModel
                    {
                        User = movieReview.User.UserName,
                        Item = movieReview.Movie.Name,
                        Content = movieReview.Content,
                        Rating = movieReview.Rating,
                        Date = movieReview.Date,
                    }).ToList();

            return reviewsAllViewModel;
        }

        public List<ReviewAllViewModel> GetAllSeasonReviews(string seasonId)
        {
            var seasonReviewsFromDb = dbContext.SeasonReviews.Where(seasonReview => seasonReview.SeasonId == seasonId);

            var reviewsAllViewModel = seasonReviewsFromDb
                    .Select(seasonReview => new ReviewAllViewModel
                    {
                        User = seasonReview.User.UserName,
                        Item = seasonReview.Season.TVShow.Name + " Season " + seasonReview.Season.SeasonNumber,
                        Content = seasonReview.Content,
                        Rating = seasonReview.Rating,
                        Date = seasonReview.Date,
                    }).ToList();

            return reviewsAllViewModel;
        }

        public CreateReviewInputModel GetMovieReview(string userId, string movieId)
        {
            var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId);

            var movieReviewInputModel = new CreateReviewInputModel
            {
                Id = movieReviewFromDb.MovieId,
                Content = movieReviewFromDb.Content,
                Rating = movieReviewFromDb.Rating,
            };

            return movieReviewInputModel;
        }

        public CreateReviewInputModel GetSeasonReview(string userId, string seasonId)
        {
            var seasonReviewFromDb = dbContext.SeasonReviews
                    .SingleOrDefault(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId);

            var seasonReviewInputModel = new CreateReviewInputModel
            {
                Id = seasonReviewFromDb.SeasonId,
                Content = seasonReviewFromDb.Content,
                Rating = seasonReviewFromDb.Rating,
            };

            return seasonReviewInputModel;
        }

        public async Task<bool> CreateMovieReviewAsync(string userId , CreateReviewInputModel input)
        {
            if (dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId))
            {
                return false;
            }

            var movieReviewForDb = new MovieReview
            {
                MovieId = input.Id,
                UserId = userId,
                Content = input.Content,
                Rating = input.Rating,
                Date = DateTime.UtcNow,
            };

            await dbContext.MovieReviews.AddAsync(movieReviewForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateSeasonReviewAsync(string userId, CreateReviewInputModel input)
        {
            if (dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == input.Id && seasonReview.UserId == userId))
            {
                return false;
            }

            var seasonReviewForDb = new SeasonReview
            {
                SeasonId = input.Id,
                UserId = userId,
                Content = input.Content,
                Rating = input.Rating,
                Date = DateTime.UtcNow,
            };

            await dbContext.SeasonReviews.AddAsync(seasonReviewForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateMovieReviewAsync(string userId, CreateReviewInputModel input)
        {
            if (!dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId))
            {
                return false;
            }

            var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId);

            movieReviewFromDb.Content = input.Content;
            movieReviewFromDb.Rating = input.Rating;
            movieReviewFromDb.Date = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateSeasonReviewAsync(string userId, CreateReviewInputModel input)
        {
            if (!dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == input.Id && seasonReview.UserId == userId))
            {
                return false;
            }

            var seasonReviewFromDb = dbContext.SeasonReviews
                    .SingleOrDefault(seasonReview => seasonReview.SeasonId == input.Id && seasonReview.UserId == userId);

            seasonReviewFromDb.Content = input.Content;
            seasonReviewFromDb.Rating = input.Rating;
            seasonReviewFromDb.Date = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMovieReviewAsync(string userId, string movieId)
        {
            if (!dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId))
            {
                return false;
            }

            var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId);

            dbContext.MovieReviews.Remove(movieReviewFromDb);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSeasonReviewAsync(string userId, string seasonId)
        {
            if (!dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId))
            {
                return false;
            }

            var seasonReviewFromDb = dbContext.SeasonReviews
                    .SingleOrDefault(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId);

            dbContext.SeasonReviews.Remove(seasonReviewFromDb);

            await dbContext.SaveChangesAsync();

            return true;
        }    
    }
}
