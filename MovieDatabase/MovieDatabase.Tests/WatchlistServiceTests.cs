using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Watchlist;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class WatchlistServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public WatchlistServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WatchlistProfile());
            });
            this.mapper = config.CreateMapper();            
        }

        [Fact]
        public async Task IsValidMovieOrTVShowIdShouldReturnCorrectResult()
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var tvShowId = tvShow1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            var movieResult = await watchlistService.IsValidMovieOrTVShowIdAsync(movieId);
            var seasonResult = await watchlistService.IsValidMovieOrTVShowIdAsync(tvShowId);
            var invalid = await watchlistService.IsValidMovieOrTVShowIdAsync("invalidId");

            Assert.True(movieResult);
            Assert.True(seasonResult);
            Assert.False(invalid);
        }

        [Fact]
        public async Task IsIdMovieOrTVShowIdShouldReturnCorrectResult()
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var movieId = movie1.Id;
            var tvShowId = tvShow1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            var movieResult = await watchlistService.IsIdMovieOrTVShowIdAsync(movieId);
            var seasonResult = await watchlistService.IsIdMovieOrTVShowIdAsync(tvShowId);
            var invalid = await watchlistService.IsIdMovieOrTVShowIdAsync("invalidId");

            Assert.Equal("Movie", movieResult);
            Assert.Equal("TV Show", seasonResult);
            Assert.Equal("Neither", invalid);
        }

        [Fact]
        public async Task MovieIsInUserWatchlistShouldReturnCorrectResult()
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
            var movieUser = new MovieUser
            {
                User = user1,
                Movie = movie1,
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieUsers.AddAsync(movieUser);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var movieId = movie1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            var movieResult = await watchlistService.MovieIsInUserWatchlistAsync(userId, movieId);
            var invalid1 = await watchlistService.MovieIsInUserWatchlistAsync("invalidUserId", movieId);
            var invalid2 = await watchlistService.MovieIsInUserWatchlistAsync(userId, "invalidMovieId");

            Assert.True(movieResult);
            Assert.False(invalid1);
            Assert.False(invalid2);
        }

        [Fact]
        public async Task TVShowIsInUserWatchlistShouldReturnCorrectResult()
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            var tvShowUser = new TVShowUser
            {
                User = user1,
                TVShow = tvShow1,
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShowUsers.AddAsync(tvShowUser);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var tvShowId = tvShow1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            var tvShowResult = await watchlistService.TVShowIsInUserWatchlistAsync(userId, tvShowId);
            var invalid1 = await watchlistService.TVShowIsInUserWatchlistAsync("invalidUserId", tvShowId);
            var invalid2 = await watchlistService.TVShowIsInUserWatchlistAsync(userId, "invalidTvShowId");

            Assert.True(tvShowResult);
            Assert.False(invalid1);
            Assert.False(invalid2);
        }

        [Fact]
        public async Task GetItemsInUserWatchlistShouldReturnEmptyListWithEmptyDb()
        {
            var watchlistService = new WatchlistService(dbContext, mapper);

            var expectedResult = new List<WatchlistAllViewModel>();

            var actualResult = await watchlistService.GetItemsInUserWatchlistAsync("userId");

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetItemsInUserWatchlistShouldReturnWatchlistProperly()
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            var season1 = new Season
            {
                TVShow = tvShow1,
                SeasonNumber = 1,
                Episodes = 20,
                LengthPerEpisode = 45,
                ReleaseDate = DateTime.Parse("24 July 2019"),
            };
            var movieUser = new MovieUser
            {
                User = user1,
                Movie = movie1,
            };
            var movieUser2 = new MovieUser
            {
                User = user2,
                Movie = movie1,
            };
            var movieReview = new MovieReview
            {
                User = user1,
                Movie = movie1,
                Content = "content1",
                Rating = 6,
                Date = DateTime.UtcNow,
            };
            var tvShowUser = new TVShowUser
            {
                User = user1,
                TVShow = tvShow1,
            };
            var seasonReview = new SeasonReview
            {
                User = user1,
                Season = season1,
                Content = "content2",
                Rating = 5,
                Date = DateTime.UtcNow,
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.MovieUsers.AddAsync(movieUser);
            await dbContext.MovieUsers.AddAsync(movieUser2);
            await dbContext.TVShowUsers.AddAsync(tvShowUser);
            await dbContext.MovieReviews.AddAsync(movieReview);
            await dbContext.SeasonReviews.AddAsync(seasonReview);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var movie1Id = movie1.Id;
            var tvShow1Id = tvShow1.Id;            

            var watchlistService = new WatchlistService(dbContext, mapper);

            var expectedResult = new List<WatchlistAllViewModel>
            {
                new WatchlistAllViewModel
                {
                    Id = movie1Id,
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Description = "description1....",
                    CoverImageLink = "cover1",
                    Category = "Movies",
                    Rating = 6,
                },
                new WatchlistAllViewModel
                {
                    Id = tvShow1Id,
                    Name = "tvShow1",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Description = "description2....",
                    CoverImageLink = "cover2",
                    Category = "TVShows",
                    Rating = 5,
                }
            };

            var actualResult = await watchlistService.GetItemsInUserWatchlistAsync(userId);

            Assert.True(actualResult.Count() == 2);

            Assert.Equal(expectedResult[0].Id, actualResult[0].Id);
            Assert.Equal(expectedResult[0].Name, actualResult[0].Name);
            Assert.Equal(expectedResult[0].ReleaseDate, actualResult[0].ReleaseDate);
            Assert.Equal(expectedResult[0].Description, actualResult[0].Description);
            Assert.Equal(expectedResult[0].CoverImageLink, actualResult[0].CoverImageLink);
            Assert.Equal(expectedResult[0].Category, actualResult[0].Category);
            Assert.Equal(expectedResult[0].Rating, actualResult[0].Rating);

            Assert.Equal(expectedResult[1].Id, actualResult[1].Id);
            Assert.Equal(expectedResult[1].Name, actualResult[1].Name);
            Assert.Equal(expectedResult[1].ReleaseDate, actualResult[1].ReleaseDate);
            Assert.Equal(expectedResult[1].Description, actualResult[1].Description);
            Assert.Equal(expectedResult[1].CoverImageLink, actualResult[1].CoverImageLink);
            Assert.Equal(expectedResult[1].Category, actualResult[1].Category);
            Assert.Equal(expectedResult[1].Rating, actualResult[1].Rating);
        }

        [Fact]
        public async Task AddMovieToUserWatchlistShouldShouldAddMovieTouserWatchlistProperly()
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
            await dbContext.Users.AddAsync(user1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var movieId = movie1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            Assert.True(dbContext.MovieUsers.Count() == 0);

            await watchlistService.AddMovieToUserWatchlistAsync(userId, movieId);

            Assert.True(dbContext.MovieUsers.Count() == 1);

            Assert.Equal(dbContext.MovieUsers.First().Movie, movie1);
            Assert.Equal(dbContext.MovieUsers.First().User, user1);
        }

        [Fact]
        public async Task AddTVShowToUserWatchlistShouldShouldAddMovieTouserWatchlistProperly()
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
            var tvShow1 = new TVShow
            {
                Name = "movie1",
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var tvShowId = tvShow1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            Assert.True(dbContext.TVShowUsers.Count() == 0);

            await watchlistService.AddTVShowToUserWatchlistAsync(userId, tvShowId);

            Assert.True(dbContext.TVShowUsers.Count() == 1);

            Assert.Equal(dbContext.TVShowUsers.First().TVShow, tvShow1);
            Assert.Equal(dbContext.TVShowUsers.First().User, user1);
        }

        [Fact]
        public async Task RemoveMovieFromUserWatchlistShouldShouldRemoveMovieFromWatchlistProperly()
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
            var movieUser = new MovieUser
            {
                User = user1,
                Movie = movie1,
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieUsers.AddAsync(movieUser);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var movieId = movie1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            Assert.True(dbContext.MovieUsers.Count() == 1);

            await watchlistService.RemoveMovieFromUserWatchlistAsync(userId, movieId);

            Assert.True(dbContext.MovieUsers.Count() == 0);
        }

        [Fact]
        public async Task RemoveTVShowFromUserWatchlistShouldShouldRemoveMovieFromWatchlistProperly()
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
            var tvShow1 = new TVShow
            {
                Name = "movie1",
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var tvShowUser = new TVShowUser
            {
                User = user1,
                TVShow = tvShow1,
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShowUsers.AddAsync(tvShowUser);
            await dbContext.SaveChangesAsync();

            var userId = user1.Id;
            var tvShowId = tvShow1.Id;

            var watchlistService = new WatchlistService(dbContext, mapper);

            Assert.True(dbContext.TVShowUsers.Count() == 1);

            await watchlistService.RemoveTVShowFromUserWatchlistAsync(userId, tvShowId);

            Assert.True(dbContext.TVShowUsers.Count() == 0);
        }
    }
}
