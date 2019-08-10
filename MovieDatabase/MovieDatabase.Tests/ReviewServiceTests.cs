using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class ReviewServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ReviewsProfile());
            });
            this.mapper = config.CreateMapper();
        }

        [Fact]
        public async Task IsValidMovieOrSeasonIdShouldReturnCorrectResult()
        {
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 5,
                ReleaseDate = DateTime.UtcNow,
                LengthPerEpisode = 35,
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var seasonId = season1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var movieResult = await reviewService.IsValidMovieOrSeasonIdAsync(movieId);
            var seasonResult = await reviewService.IsValidMovieOrSeasonIdAsync(seasonId);
            var invalid = await reviewService.IsValidMovieOrSeasonIdAsync("invalidId");

            Assert.True(movieResult);
            Assert.True(seasonResult);
            Assert.False(invalid);
        }

        [Fact]
        public async Task IsIdMovieOrSeasonIdShouldReturnCorrectResult()
        {
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 5,
                ReleaseDate = DateTime.UtcNow,
                LengthPerEpisode = 35,
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var seasonId = season1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var movieResult = await reviewService.IsIdMovieOrSeasonIdAsync(movieId);
            var seasonResult = await reviewService.IsIdMovieOrSeasonIdAsync(seasonId);
            var invalid = await reviewService.IsIdMovieOrSeasonIdAsync("invalidId");

            Assert.Equal("Movie", movieResult);
            Assert.Equal("Season", seasonResult);
            Assert.Equal("Neither", invalid);
        }

        [Fact]
        public async Task ReviewExistsShouldReturnCorrectResult()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 5,
                ReleaseDate = DateTime.UtcNow,
                LengthPerEpisode = 35,
            };
            var movieReview = new MovieReview
            {
                Movie = movie1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.UtcNow,
            };
            var seasonReview = new SeasonReview
            {
                Season = season1,
                User = user1,
                Content = "content2",
                Rating = 7,
                Date = DateTime.UtcNow,
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.MovieReviews.AddAsync(movieReview);
            await dbContext.SeasonReviews.AddAsync(seasonReview);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var movieId = movie1.Id;
            var seasonId = season1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var movieResult = await reviewService.ReviewExistsAsync(userId, movieId);
            var seasonResult = await reviewService.ReviewExistsAsync(userId, seasonId);
            var invalid = await reviewService.ReviewExistsAsync(userId, "invalidId");

            Assert.True(movieResult);
            Assert.True(seasonResult);
            Assert.False(invalid);
        }

        [Fact]
        public async Task GetAllMovieReviewsShouldReturnEmptyListWithEmptyDb()
        {
            var reviewService = new ReviewService(dbContext, mapper);

            var expectedResult = new List<ReviewAllViewModel>();

            var actualResult = await reviewService.GetAllMovieReviewsAsync("id");

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllMovieReviewsShouldReturnAllReviewsProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var user2 = new MovieDatabaseUser
            {
                UserName = "username2",
                Email = "email2@mail.com",
                AvatarLink = "avatar2",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var movieReview1 = new MovieReview
            {
                Movie = movie1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            var movieReview2 = new MovieReview
            {
                Movie = movie1,
                User = user2,
                Content = "content2",
                Rating = 9,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieReviews.AddAsync(movieReview1);
            await dbContext.MovieReviews.AddAsync(movieReview2);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var expectedResult = new List<ReviewAllViewModel>
            {
                new ReviewAllViewModel
                {
                    Item = "movie1",
                    User = "username",
                    Content = "content1",
                    Rating = 8,
                    Date = DateTime.Parse("27 July 2019"),
                },
                new ReviewAllViewModel
                {
                    Item = "movie1",
                    User = "username2",
                    Content = "content2",
                    Rating = 9,
                    Date = DateTime.Parse("27 July 2019"),
                },
            };

            var actualResult = await reviewService.GetAllMovieReviewsAsync(movieId);

            Assert.True(actualResult.Count() == 2);

            Assert.Equal(expectedResult[0].Item, actualResult[0].Item);
            Assert.Equal(expectedResult[0].User, actualResult[0].User);
            Assert.Equal(expectedResult[0].Content, actualResult[0].Content);
            Assert.Equal(expectedResult[0].Rating, actualResult[0].Rating);
            Assert.Equal(expectedResult[0].Date, actualResult[0].Date);

            Assert.Equal(expectedResult[1].Item, actualResult[1].Item);
            Assert.Equal(expectedResult[1].User, actualResult[1].User);
            Assert.Equal(expectedResult[1].Content, actualResult[1].Content);
            Assert.Equal(expectedResult[1].Rating, actualResult[1].Rating);
            Assert.Equal(expectedResult[1].Date, actualResult[1].Date);
        }

        [Fact]
        public async Task GetAllSeasoReviewsShouldReturnEmptyListWithEmptyDb()
        {
            var reviewService = new ReviewService(dbContext, mapper);

            var expectedResult = new List<ReviewAllViewModel>();

            var actualResult = await reviewService.GetAllSeasonReviewsAsync("id");

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllSeasonReviewsShouldReturnAllReviewsProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var user2 = new MovieDatabaseUser
            {
                UserName = "username2",
                Email = "email2@mail.com",
                AvatarLink = "avatar2",
            };
            var tvShow1 = new TVShow
            {
                Name = "tvshow1",
                Description = "description",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 9,
                LengthPerEpisode = 56,
                ReleaseDate = DateTime.Parse("11 January 1998"),
                TVShow = tvShow1,
            };
            var seasonReview1 = new SeasonReview
            {
                Season = season1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            var seasonReview2 = new SeasonReview
            {
                Season = season1,
                User = user2,
                Content = "content2",
                Rating = 9,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview1);
            await dbContext.SeasonReviews.AddAsync(seasonReview2);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var expectedResult = new List<ReviewAllViewModel>
            {
                new ReviewAllViewModel
                {
                    Item = "tvshow1 Season 1",
                    User = "username",
                    Content = "content1",
                    Rating = 8,
                    Date = DateTime.Parse("27 July 2019"),
                },
                new ReviewAllViewModel
                {
                    Item = "tvshow1 Season 1",
                    User = "username2",
                    Content = "content2",
                    Rating = 9,
                    Date = DateTime.Parse("27 July 2019"),
                },
            };

            var actualResult = await reviewService.GetAllSeasonReviewsAsync(seasonId);

            Assert.True(actualResult.Count() == 2);

            Assert.Equal(expectedResult[0].Item, actualResult[0].Item);
            Assert.Equal(expectedResult[0].User, actualResult[0].User);
            Assert.Equal(expectedResult[0].Content, actualResult[0].Content);
            Assert.Equal(expectedResult[0].Rating, actualResult[0].Rating);
            Assert.Equal(expectedResult[0].Date, actualResult[0].Date);

            Assert.Equal(expectedResult[1].Item, actualResult[1].Item);
            Assert.Equal(expectedResult[1].User, actualResult[1].User);
            Assert.Equal(expectedResult[1].Content, actualResult[1].Content);
            Assert.Equal(expectedResult[1].Rating, actualResult[1].Rating);
            Assert.Equal(expectedResult[1].Date, actualResult[1].Date);
        }

        [Fact]
        public async Task GetMovieReviewShouldReturnNullWithEmptyDb()
        {
            var reviewService = new ReviewService(dbContext, mapper);

            var actualResult = await reviewService.GetMovieReviewAsync("userId", "movieId");

            Assert.Null(actualResult);
        }

        [Fact]
        public async Task GetMovieReviewShouldReturnCorrectModel()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var movieReview1 = new MovieReview
            {
                Movie = movie1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieReviews.AddAsync(movieReview1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var expectedResult = new CreateReviewInputModel
            {
                Id = movieId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.GetMovieReviewAsync(userId, movieId);

            Assert.Equal(expectedResult.GetType(), actualResult.GetType());
            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Content, actualResult.Content);
            Assert.Equal(expectedResult.Rating, actualResult.Rating);
        }

        [Fact]
        public async Task GetSeasonReviewShouldReturnNullWithEmptyDb()
        {
            var reviewService = new ReviewService(dbContext, mapper);

            var actualResult = await reviewService.GetSeasonReviewAsync("userId", "seasonId");

            Assert.Null(actualResult);
        }

        [Fact]
        public async Task GetSeasonReviewShouldReturnCorrectModel()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };
            var seasonReview1 = new SeasonReview
            {
                Season = season1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var expectedResult = new CreateReviewInputModel
            {
                Id = seasonId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.GetSeasonReviewAsync(userId, seasonId);

            Assert.Equal(expectedResult.GetType(), actualResult.GetType());
            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Content, actualResult.Content);
            Assert.Equal(expectedResult.Rating, actualResult.Rating);
        }

        [Fact]
        public async Task CreateMovieReviewShouldReturnFalseIfReviewAlreadyExists()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var movieReview1 = new MovieReview
            {
                Movie = movie1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieReviews.AddAsync(movieReview1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = movieId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.CreateMovieReviewAsync(userId, input);

            Assert.False(actualResult);
            Assert.True(dbContext.MovieReviews.Count() == 1);
        }

        [Fact]
        public async Task CreateMovieReviewShouldCreateReviewProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = movieId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.CreateMovieReviewAsync(userId, input);

            Assert.True(actualResult);
            Assert.True(dbContext.MovieReviews.Count() == 1);

            Assert.Equal(8, dbContext.MovieReviews.First().Rating);
            Assert.Equal("content1", dbContext.MovieReviews.First().Content);
            Assert.Equal(userId, dbContext.MovieReviews.First().UserId);
            Assert.Equal(movieId, dbContext.MovieReviews.First().MovieId);
        }

        [Fact]
        public async Task CreateSeasonReviewShouldReturnFalseIfReviewAlreadyExists()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };
            var seasonReview1 = new SeasonReview
            {
                Season = season1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = seasonId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.CreateSeasonReviewAsync(userId, input);

            Assert.False(actualResult);
            Assert.True(dbContext.SeasonReviews.Count() == 1);
        }

        [Fact]
        public async Task CreateSeasonReviewShouldCreateReviewProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = seasonId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.CreateSeasonReviewAsync(userId, input);

            Assert.True(actualResult);
            Assert.True(dbContext.SeasonReviews.Count() == 1);

            Assert.Equal(8, dbContext.SeasonReviews.First().Rating);
            Assert.Equal("content1", dbContext.SeasonReviews.First().Content);
            Assert.Equal(userId, dbContext.SeasonReviews.First().UserId);
            Assert.Equal(seasonId, dbContext.SeasonReviews.First().SeasonId);
        }

        [Fact]
        public async Task UpdateMovieReviewShouldReturnFalseIfReviewDoesntExist()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };

            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = movieId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.UpdateMovieReviewAsync(userId, input);

            Assert.False(actualResult);
            Assert.True(dbContext.MovieReviews.Count() == 0);
        }

        [Fact]
        public async Task UpdateMovieReviewShouldUpdateReviewProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var movieReview1 = new MovieReview
            {
                Movie = movie1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieReviews.AddAsync(movieReview1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = movieId,
                Content = "content2",
                Rating = 9,
            };

            var actualResult = await reviewService.UpdateMovieReviewAsync(userId, input);

            Assert.True(actualResult);
            Assert.True(dbContext.MovieReviews.Count() == 1);

            Assert.Equal(9, dbContext.MovieReviews.First().Rating);
            Assert.Equal("content2", dbContext.MovieReviews.First().Content);
            Assert.Equal(userId, dbContext.MovieReviews.First().UserId);
            Assert.Equal(movieId, dbContext.MovieReviews.First().MovieId);
        }

        [Fact]
        public async Task UpdateSeasonReviewShouldReturnFalseIfReviewDoesntExist()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };

            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = seasonId,
                Content = "content1",
                Rating = 8,
            };

            var actualResult = await reviewService.UpdateSeasonReviewAsync(userId, input);

            Assert.False(actualResult);
            Assert.True(dbContext.SeasonReviews.Count() == 0);
        }

        [Fact]
        public async Task UpdateSeasonReviewShouldUpdateReviewProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };
            var seasonReview1 = new SeasonReview
            {
                Season = season1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var input = new CreateReviewInputModel
            {
                Id = seasonId,
                Content = "content2",
                Rating = 7,
            };

            var actualResult = await reviewService.UpdateSeasonReviewAsync(userId, input);

            Assert.True(actualResult);
            Assert.True(dbContext.SeasonReviews.Count() == 1);

            Assert.Equal(7, dbContext.SeasonReviews.First().Rating);
            Assert.Equal("content2", dbContext.SeasonReviews.First().Content);
            Assert.Equal(userId, dbContext.SeasonReviews.First().UserId);
            Assert.Equal(seasonId, dbContext.SeasonReviews.First().SeasonId);
        }

        [Fact]
        public async Task DeleteMovieReviewShouldReturnFalseIfReviewDoesntExist()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };

            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var actualResult = await reviewService.DeleteMovieReviewAsync(userId, movieId);

            Assert.False(actualResult);
            Assert.True(dbContext.MovieReviews.Count() == 0);
        }

        [Fact]
        public async Task DeleteMovieReviewShouldUpdateReviewProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = new Genre { Name = "genre1" },
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var movieReview1 = new MovieReview
            {
                Movie = movie1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieReviews.AddAsync(movieReview1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            Assert.True(dbContext.MovieReviews.Count() == 1);

            var actualResult = await reviewService.DeleteMovieReviewAsync(userId, movieId);

            Assert.True(actualResult);
            Assert.True(dbContext.MovieReviews.Count() == 0);
        }

        [Fact]
        public async Task DeleteSeasonReviewShouldReturnFalseIfReviewDoesntExist()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };

            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            var actualResult = await reviewService.DeleteSeasonReviewAsync(userId, seasonId);

            Assert.False(actualResult);
            Assert.True(dbContext.SeasonReviews.Count() == 0);
        }

        [Fact]
        public async Task DeleteSeasonReviewShouldUpdateReviewProperly()
        {
            var user1 = new MovieDatabaseUser
            {
                UserName = "username",
                Email = "email@mail.com",
                AvatarLink = "avatar",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 10,
                LengthPerEpisode = 29,
                ReleaseDate = DateTime.Parse("28 October 2016"),
            };
            var seasonReview1 = new SeasonReview
            {
                Season = season1,
                User = user1,
                Content = "content1",
                Rating = 8,
                Date = DateTime.Parse("27 July 2019"),
            };
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview1);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;
            var userId = user1.Id;

            var reviewService = new ReviewService(dbContext, mapper);

            Assert.True(dbContext.SeasonReviews.Count() == 1);

            var actualResult = await reviewService.DeleteSeasonReviewAsync(userId, seasonId);

            Assert.True(actualResult);
            Assert.True(dbContext.SeasonReviews.Count() == 0);
        }
    }
}
