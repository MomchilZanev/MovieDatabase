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
            if (dbContext.Movies.Any(m => m.Id == itemId) || dbContext.Seasons.Any(t => t.Id == itemId))
            {
                return true;
            }

            return false;
        }

        public bool ReviewExists(string userId, string itemId)
        {
            if (dbContext.MovieReviews.Any(mr => mr.MovieId == itemId && mr.UserId == userId) ||
                dbContext.SeasonReviews.Any(sr => sr.SeasonId == itemId && sr.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public List<ReviewAllViewModel> GetAllMovieOrSeasonReviews(string itemId)
        {
            if (dbContext.MovieReviews.Any(mr => mr.MovieId == itemId))
            {
                var movieReviews = dbContext.MovieReviews
                    .Where(mr => mr.MovieId == itemId).Select(mr => new ReviewAllViewModel
                    {
                        User = mr.User.UserName,
                        Item = mr.Movie.Name,
                        Content = mr.Content,
                        Rating = mr.Rating,
                        Date = mr.Date,
                    }).ToList();

                return movieReviews;
            }
            else if (dbContext.SeasonReviews.Any(sr => sr.SeasonId == itemId))
            {
                var seasonReviews = dbContext.SeasonReviews
                    .Where(sr => sr.SeasonId == itemId).Select(sr => new ReviewAllViewModel
                    {
                        User = sr.User.UserName,
                        Item = sr.Season.TVShow.Name + " Season " + sr.Season.SeasonNumber,
                        Content = sr.Content,
                        Rating = sr.Rating,
                        Date = sr.Date,
                    }).ToList();

                return seasonReviews;
            }

            return new List<ReviewAllViewModel>();
        }

        public CreateReviewInputModel GetUserReview(string userId, string itemId)
        {
            if (dbContext.MovieReviews.Any(mr => mr.MovieId == itemId && mr.UserId == userId))
            {
                var movieReview = dbContext.MovieReviews.SingleOrDefault(mr => mr.MovieId == itemId && mr.UserId == userId);

                return new CreateReviewInputModel
                {
                    Id = movieReview.MovieId,
                    Content = movieReview.Content,
                    Rating = movieReview.Rating,
                };
            }
            else if (dbContext.SeasonReviews.Any(sr => sr.SeasonId == itemId && sr.UserId == userId))
            {
                var seasonReview = dbContext.SeasonReviews.SingleOrDefault(mr => mr.SeasonId == itemId && mr.UserId == userId);

                return new CreateReviewInputModel
                {
                    Id = seasonReview.SeasonId,
                    Content = seasonReview.Content,
                    Rating = seasonReview.Rating,
                };
            }

            return null;
        }

        public string CreateUserReview(string userId, string itemId, string content, int rating)
        {
            if (dbContext.Movies.Any(m => m.Id == itemId))
            {
                var movieReview = new MovieReview
                {
                    MovieId = itemId,
                    UserId = userId,
                    Content = content,
                    Rating = rating,
                    Date = DateTime.UtcNow,                    
                };

                dbContext.MovieReviews.Add(movieReview);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.Seasons.Any(s => s.Id == itemId))
            {
                var seasonReview = new SeasonReview
                {
                    SeasonId = itemId,
                    UserId = userId,
                    Content = content,
                    Rating = rating,
                    Date = DateTime.UtcNow,
                };

                dbContext.SeasonReviews.Add(seasonReview);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }

        public string UpdateUserReview(string userId, string itemId, string content, int rating)
        {
            if (dbContext.Movies.Any(m => m.Id == itemId))
            {
                var movieReview = dbContext.MovieReviews.SingleOrDefault(mr => mr.MovieId == itemId && mr.UserId == userId);

                movieReview.Content = content;
                movieReview.Rating = rating;
                movieReview.Date = DateTime.UtcNow;

                dbContext.Update(movieReview);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.Seasons.Any(s => s.Id == itemId))
            {
                var seasonReview = dbContext.SeasonReviews.SingleOrDefault(mr => mr.SeasonId == itemId && mr.UserId == userId);

                seasonReview.Content = content;
                seasonReview.Rating = rating;
                seasonReview.Date = DateTime.UtcNow;

                dbContext.Update(seasonReview);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }

        public string DeleteUserReview(string userId, string itemId)
        {
            if (dbContext.Movies.Any(m => m.Id == itemId))
            {
                var movieReview = dbContext.MovieReviews.SingleOrDefault(mr => mr.MovieId == itemId && mr.UserId == userId);

                dbContext.MovieReviews.Remove(movieReview);

                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.Seasons.Any(s => s.Id == itemId))
            {
                var seasonReview = dbContext.SeasonReviews.SingleOrDefault(mr => mr.SeasonId == itemId && mr.UserId == userId);

                dbContext.SeasonReviews.Remove(seasonReview);

                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }        
    }
}
