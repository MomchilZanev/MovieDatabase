using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool ReviewExists(string userId, string itemId)
        {
            if (dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId) ||
                dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public List<ReviewAllViewModel> GetAllMovieOrSeasonReviews(string itemId)
        {
            if (dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == itemId))
            {
                var movieReviewsFromDb = dbContext.MovieReviews
                    .Where(movieReview => movieReview.MovieId == itemId)
                    .Select(movieReview => new ReviewAllViewModel
                    {
                        User = movieReview.User.UserName,
                        Item = movieReview.Movie.Name,
                        Content = movieReview.Content,
                        Rating = movieReview.Rating,
                        Date = movieReview.Date,
                    }).ToList();

                return movieReviewsFromDb;
            }
            else if (dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == itemId))
            {
                var seasonReviewsFromDb = dbContext.SeasonReviews
                    .Where(seasonReview => seasonReview.SeasonId == itemId)
                    .Select(seasonReview => new ReviewAllViewModel
                    {
                        User = seasonReview.User.UserName,
                        Item = seasonReview.Season.TVShow.Name + " Season " + seasonReview.Season.SeasonNumber,
                        Content = seasonReview.Content,
                        Rating = seasonReview.Rating,
                        Date = seasonReview.Date,
                    }).ToList();

                return seasonReviewsFromDb;
            }

            return new List<ReviewAllViewModel>();
        }

        public CreateReviewInputModel GetUserReview(string userId, string itemId)
        {
            if (dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId))
            {
                var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId);

                return new CreateReviewInputModel
                {
                    Id = movieReviewFromDb.MovieId,
                    Content = movieReviewFromDb.Content,
                    Rating = movieReviewFromDb.Rating,
                };
            }
            else if (dbContext.SeasonReviews.Any(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId))
            {
                var seasonReviewFromDb = dbContext.SeasonReviews
                    .SingleOrDefault(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId);

                return new CreateReviewInputModel
                {
                    Id = seasonReviewFromDb.SeasonId,
                    Content = seasonReviewFromDb.Content,
                    Rating = seasonReviewFromDb.Rating,
                };
            }

            return null;
        }

        public string CreateUserReview(string userId, string itemId, string content, int rating)
        {
            if (dbContext.Movies.Any(movie => movie.Id == itemId))
            {
                var movieReviewForDb = new MovieReview
                {
                    MovieId = itemId,
                    UserId = userId,
                    Content = content,
                    Rating = rating,
                    Date = DateTime.UtcNow,                    
                };

                dbContext.MovieReviews.Add(movieReviewForDb);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.Seasons.Any(s => s.Id == itemId))
            {
                var seasonReviewForDb = new SeasonReview
                {
                    SeasonId = itemId,
                    UserId = userId,
                    Content = content,
                    Rating = rating,
                    Date = DateTime.UtcNow,
                };

                dbContext.SeasonReviews.Add(seasonReviewForDb);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }

        public string UpdateUserReview(string userId, string itemId, string content, int rating)
        {
            if (dbContext.Movies.Any(movie => movie.Id == itemId))
            {
                var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId);

                movieReviewFromDb.Content = content;
                movieReviewFromDb.Rating = rating;
                movieReviewFromDb.Date = DateTime.UtcNow;

                dbContext.Update(movieReviewFromDb);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.Seasons.Any(season => season.Id == itemId))
            {
                var seasonReviewForDb = dbContext.SeasonReviews.SingleOrDefault(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId);

                seasonReviewForDb.Content = content;
                seasonReviewForDb.Rating = rating;
                seasonReviewForDb.Date = DateTime.UtcNow;

                dbContext.Update(seasonReviewForDb);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }

        public string DeleteUserReview(string userId, string itemId)
        {
            if (dbContext.Movies.Any(movie => movie.Id == itemId))
            {
                var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId);

                dbContext.MovieReviews.Remove(movieReviewFromDb);

                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.Seasons.Any(season => season.Id == itemId))
            {
                var seasonReviewFromDb = dbContext.SeasonReviews
                    .SingleOrDefault(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId);

                dbContext.SeasonReviews.Remove(seasonReviewFromDb);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }        
    }
}
