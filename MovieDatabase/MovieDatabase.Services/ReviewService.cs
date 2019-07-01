using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services.Contracts;
using System;
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

        public bool IsValidId(string itemId)
        {
            if (dbContext.Movies.Any(m => m.Id == itemId) || dbContext.Seasons.Any(t => t.Id == itemId))
            {
                return true;
            }

            return false;
        }

        public bool Exists(string userId, string itemId)
        {
            if (dbContext.MovieReviews.Any(mr => mr.MovieId == itemId && mr.UserId == userId) ||
                dbContext.SeasonReviews.Any(sr => sr.SeasonId == itemId && sr.UserId == userId))
            {
                return true;
            }

            return false;
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
    }
}
