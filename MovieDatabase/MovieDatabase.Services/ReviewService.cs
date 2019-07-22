using AutoMapper;
using MovieDatabase.Common;
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
        private readonly IMapper mapper;

        public ReviewService(MovieDatabaseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
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
                return GlobalConstants.Movie;
            }
            else if (dbContext.Seasons.Any(season => season.Id == id))
            {
                return GlobalConstants.Season;
            }

            return GlobalConstants.Neither;
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
            var movieReviewsFromDb = dbContext.MovieReviews.Where(movieReview => movieReview.MovieId == movieId).ToList();

            var reviewsAllViewModel = mapper.Map<List<MovieReview>, List<ReviewAllViewModel>>(movieReviewsFromDb);

            return reviewsAllViewModel;
        }

        public List<ReviewAllViewModel> GetAllSeasonReviews(string seasonId)
        {
            var seasonReviewsFromDb = dbContext.SeasonReviews.Where(seasonReview => seasonReview.SeasonId == seasonId).ToList();

            var reviewsAllViewModel = mapper.Map<List<SeasonReview>, List<ReviewAllViewModel>>(seasonReviewsFromDb);

            return reviewsAllViewModel;
        }

        public CreateReviewInputModel GetMovieReview(string userId, string movieId)
        {
            var movieReviewFromDb = dbContext.MovieReviews
                    .SingleOrDefault(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId);

            var movieReviewInputModel = mapper.Map<MovieReview, CreateReviewInputModel>(movieReviewFromDb);

            return movieReviewInputModel;
        }

        public CreateReviewInputModel GetSeasonReview(string userId, string seasonId)
        {
            var seasonReviewFromDb = dbContext.SeasonReviews
                    .SingleOrDefault(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId);

            var seasonReviewInputModel = mapper.Map<SeasonReview, CreateReviewInputModel>(seasonReviewFromDb);

            return seasonReviewInputModel;
        }

        public async Task<bool> CreateMovieReviewAsync(string userId , CreateReviewInputModel input)
        {
            if (dbContext.MovieReviews.Any(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId))
            {
                return false;
            }

            var movieReviewForDb = mapper.Map<CreateReviewInputModel, MovieReview>(input);
            movieReviewForDb.UserId = userId;

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

            var seasonReviewForDb = mapper.Map<CreateReviewInputModel, SeasonReview>(input);
            seasonReviewForDb.UserId = userId;

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
