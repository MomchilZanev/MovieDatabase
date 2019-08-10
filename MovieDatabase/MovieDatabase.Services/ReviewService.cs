using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> IsValidMovieOrSeasonIdAsync(string itemId)
        {
            if (await dbContext.Movies.AnyAsync(movie => movie.Id == itemId) || await dbContext.Seasons.AnyAsync(t => t.Id == itemId))
            {
                return true;
            }

            return false;
        }

        public async Task<string> IsIdMovieOrSeasonIdAsync(string id)
        {
            if (await dbContext.Movies.AnyAsync(movie => movie.Id == id))
            {
                return GlobalConstants.Movie;
            }
            else if (await dbContext.Seasons.AnyAsync(season => season.Id == id))
            {
                return GlobalConstants.Season;
            }

            return GlobalConstants.Neither;
        }

        public async Task<bool> ReviewExistsAsync(string userId, string itemId)
        {
            if (await dbContext.MovieReviews.AnyAsync(movieReview => movieReview.MovieId == itemId && movieReview.UserId == userId) ||
                await dbContext.SeasonReviews.AnyAsync(seasonReview => seasonReview.SeasonId == itemId && seasonReview.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public async Task<List<ReviewAllViewModel>> GetAllMovieReviewsAsync(string movieId)
        {
            var movieReviewsFromDb = await dbContext.MovieReviews.Where(movieReview => movieReview.MovieId == movieId).ToListAsync();

            var reviewsAllViewModel = mapper.Map<List<MovieReview>, List<ReviewAllViewModel>>(movieReviewsFromDb);

            return reviewsAllViewModel;
        }

        public async Task<List<ReviewAllViewModel>> GetAllSeasonReviewsAsync(string seasonId)
        {
            var seasonReviewsFromDb = await dbContext.SeasonReviews.Where(seasonReview => seasonReview.SeasonId == seasonId).ToListAsync();

            var reviewsAllViewModel = mapper.Map<List<SeasonReview>, List<ReviewAllViewModel>>(seasonReviewsFromDb);

            return reviewsAllViewModel;
        }

        public async Task<CreateReviewInputModel> GetMovieReviewAsync(string userId, string movieId)
        {
            var movieReviewFromDb = await dbContext.MovieReviews
                    .SingleOrDefaultAsync(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId);

            var movieReviewInputModel = mapper.Map<MovieReview, CreateReviewInputModel>(movieReviewFromDb);

            return movieReviewInputModel;
        }

        public async Task<CreateReviewInputModel> GetSeasonReviewAsync(string userId, string seasonId)
        {
            var seasonReviewFromDb = await dbContext.SeasonReviews
                    .SingleOrDefaultAsync(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId);

            var seasonReviewInputModel = mapper.Map<SeasonReview, CreateReviewInputModel>(seasonReviewFromDb);

            return seasonReviewInputModel;
        }

        public async Task<bool> CreateMovieReviewAsync(string userId, CreateReviewInputModel input)
        {
            if (await dbContext.MovieReviews.AnyAsync(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId))
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
            if (await dbContext.SeasonReviews.AnyAsync(seasonReview => seasonReview.SeasonId == input.Id && seasonReview.UserId == userId))
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
            if (!await dbContext.MovieReviews.AnyAsync(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId))
            {
                return false;
            }

            var movieReviewFromDb = await dbContext.MovieReviews
                    .SingleOrDefaultAsync(movieReview => movieReview.MovieId == input.Id && movieReview.UserId == userId);

            movieReviewFromDb.Content = input.Content;
            movieReviewFromDb.Rating = input.Rating;
            movieReviewFromDb.Date = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateSeasonReviewAsync(string userId, CreateReviewInputModel input)
        {
            if (!await dbContext.SeasonReviews.AnyAsync(seasonReview => seasonReview.SeasonId == input.Id && seasonReview.UserId == userId))
            {
                return false;
            }

            var seasonReviewFromDb = await dbContext.SeasonReviews
                    .SingleOrDefaultAsync(seasonReview => seasonReview.SeasonId == input.Id && seasonReview.UserId == userId);

            seasonReviewFromDb.Content = input.Content;
            seasonReviewFromDb.Rating = input.Rating;
            seasonReviewFromDb.Date = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMovieReviewAsync(string userId, string movieId)
        {
            if (!await dbContext.MovieReviews.AnyAsync(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId))
            {
                return false;
            }

            var movieReviewFromDb = await dbContext.MovieReviews
                    .SingleOrDefaultAsync(movieReview => movieReview.MovieId == movieId && movieReview.UserId == userId);

            dbContext.MovieReviews.Remove(movieReviewFromDb);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSeasonReviewAsync(string userId, string seasonId)
        {
            if (!await dbContext.SeasonReviews.AnyAsync(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId))
            {
                return false;
            }

            var seasonReviewFromDb = await dbContext.SeasonReviews
                    .SingleOrDefaultAsync(seasonReview => seasonReview.SeasonId == seasonId && seasonReview.UserId == userId);

            dbContext.SeasonReviews.Remove(seasonReviewFromDb);

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
