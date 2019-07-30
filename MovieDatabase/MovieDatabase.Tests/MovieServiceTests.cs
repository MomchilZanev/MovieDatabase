using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Models.ViewModels.Movie;
using MovieDatabase.Services;
using MovieDatabase.Services.Contracts;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class MovieServiceTests
    {
        [Fact]
        public async Task GetAllMoviesShouldReturnEmptyListIfDbIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllMovies_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();
            watchlistService.Setup(w => w.MovieIsInUserWatchlistAsync("", ""))
                        .ReturnsAsync(false);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<MovieAllViewModel>();

            var actualResult = await movieService.GetAllMoviesAsync();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllMoviesShouldReturnAllMoviesProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllMovies_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

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
            var movie2 = new Movie
            {
                Name = "movie2",
                ReleaseDate = DateTime.Parse("25 July 2019"),
                Description = "description2",
                Length = 170,
                Genre = new Genre { Name = "genre2" },
                Director = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.Movies.AddAsync(movie2);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();
            watchlistService.Setup(w => w.MovieIsInUserWatchlistAsync("", ""))
                        .ReturnsAsync(false);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("25 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                }
            };

            var actualResult = await movieService.GetAllMoviesAsync();

            Assert.True(actualResult.Count() == 2);
            Assert.True(expectedResult[0].Name == actualResult[0].Name);
            Assert.True(expectedResult[0].ReleaseDate == actualResult[0].ReleaseDate);
            Assert.True(expectedResult[0].Genre == actualResult[0].Genre);
            Assert.True(expectedResult[0].CoverImageLink == actualResult[0].CoverImageLink);
            Assert.True(expectedResult[0].Rating == actualResult[0].Rating);
            Assert.True(expectedResult[0].TotalReviews == actualResult[0].TotalReviews);
            Assert.True(expectedResult[0].Watchlisted == actualResult[0].Watchlisted);
            Assert.True(expectedResult[1].Name == actualResult[1].Name);
            Assert.True(expectedResult[1].ReleaseDate == actualResult[1].ReleaseDate);
            Assert.True(expectedResult[1].Genre == actualResult[1].Genre);
            Assert.True(expectedResult[1].CoverImageLink == actualResult[1].CoverImageLink);
            Assert.True(expectedResult[1].Rating == actualResult[1].Rating);
            Assert.True(expectedResult[1].TotalReviews == actualResult[1].TotalReviews);
            Assert.True(expectedResult[1].Watchlisted == actualResult[1].Watchlisted);
        }

        [Fact]
        public async Task GetAllMovieNamesShouldReturnEmptyListIfDbIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllMovieNames_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<MovieNameViewModel>();

            var actualResult = await movieService.GetAllMovieNamesAsync();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllMovieNamesShouldReturnAllMovieNamesProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllMovieNames_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

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
            var movie2 = new Movie
            {
                Name = "movie2",
                ReleaseDate = DateTime.Parse("25 July 2019"),
                Description = "description2",
                Length = 170,
                Genre = new Genre { Name = "genre2" },
                Director = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.Movies.AddAsync(movie2);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<MovieNameViewModel>
            {
                new MovieNameViewModel
                {
                    Name = "movie1",
                },
                new MovieNameViewModel
                {
                    Name = "movie2",
                }
            };

            var actualResult = await movieService.GetAllMovieNamesAsync();

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(expectedResult[0].GetType(), actualResult[0].GetType());
            Assert.True(expectedResult[0].Name == actualResult[0].Name);
            Assert.True(expectedResult[1].Name == actualResult[1].Name);
        }

        [Theory]
        [InlineData("genre1", "movie1", 1)]
        [InlineData("genre2", "movie2", 1)]
        [InlineData("genre3", null, 0)]
        public async Task FilterMoviesByGenreShouldFilterMoviesProperly(string genre, string expectedMovie, int expectedCount)
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "FilterMoviesByGenre_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            await dbContext.Genres.AddRangeAsync(new[]
            {
                new Genre{ Name = "genre1" },
                new Genre{ Name = "genre2" },
                new Genre{ Name = "genre3" }
            });
            await dbContext.SaveChangesAsync();

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("25 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = await movieService.FilterMoviesByGenreAsync(input, genre);

            Assert.True(actualResult.Count() == expectedCount);
            if (actualResult.Count > 0)
            {
                Assert.True(actualResult.First().Name == expectedMovie);
            }
        }

        [Fact]
        public async Task FilterMoviesByGenreShouldReturnInputIfGenreGivenIsNotInDb()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "FilterMoviesByGenre_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("25 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<MovieNameViewModel>();

            var actualResult = await movieService.FilterMoviesByGenreAsync(input, "genre");

            Assert.True(actualResult.Count() == 2);
        }

        [Fact]
        public void OrderMoviesShouldReturnMoviesOrderedByNewest()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderMovies_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("25 July 2018"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 8,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("25 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 8,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = movieService.OrderMovies(input, GlobalConstants.moviesTvShowsOrderByRelease);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderMoviesShouldReturnMoviesOrderedByRating()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderMovies_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("25 July 2018"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 7,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("25 July 2018"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 8,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = movieService.OrderMovies(input, GlobalConstants.moviesTvShowsOrderByRating);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderMoviesShouldReturnMoviesOrderedBypopularity()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderMovies_Db_3")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("25 July 2019"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("25 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 1,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = movieService.OrderMovies(input, GlobalConstants.moviesTvShowsOrderByPopularity);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderMoviesShouldReturnUnreleasedMoviesOrderedByOldest()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderMovies_Db_4")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie3",
                    ReleaseDate = DateTime.Parse("25 July 2021"),
                    Genre = "genre3",
                    CoverImageLink = "cover3",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 1,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("25 July 2020"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = movieService.OrderMovies(input, GlobalConstants.moviesTvShowsShowComingSoon);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderMoviesShouldReturnInputIfOrderByIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderMovies_Db_5")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<MovieAllViewModel>
            {
                new MovieAllViewModel
                {
                    Name = "movie3",
                    ReleaseDate = DateTime.Parse("25 July 2021"),
                    Genre = "genre3",
                    CoverImageLink = "cover3",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie2",
                    ReleaseDate = DateTime.Parse("24 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 1,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
                new MovieAllViewModel
                {
                    Name = "movie1",
                    ReleaseDate = DateTime.Parse("25 July 2020"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
            };

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = movieService.OrderMovies(input, "invalid orderBy");

            Assert.True(actualResult.Count() == 3);
            Assert.Equal(input[0], actualResult[0]);
            Assert.Equal(input[1], actualResult[1]);
            Assert.Equal(input[2], actualResult[2]);
        }

        [Fact]
        public async Task GetMovieAndDetailsByIdShouldThrowExceptionIfDbIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetMovieAndDetailsById_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => movieService.GetMovieAndDetailsByIdAsync("id"));
        }

        [Fact]
        public async Task GetMovieAndDetailsByIdShouldThrowExceptionIfIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetMovieAndDetailsById_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

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
            var movie2 = new Movie
            {
                Name = "movie2",
                ReleaseDate = DateTime.Parse("25 July 2019"),
                Description = "description2",
                Length = 170,
                Genre = new Genre { Name = "genre2" },
                Director = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.Movies.AddAsync(movie2);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => movieService.GetMovieAndDetailsByIdAsync("invalid Id"));
        }

        [Fact]
        public async Task GetMovieAndDetailsByIdShouldReturnCorrectModel()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetMovieAndDetailsById_Db_3")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var user1 = new MovieDatabaseUser
            {
                UserName = "user1",
                AvatarLink = "img",
                Email = "email@mail.com",
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
                Cast = new List<MovieRole>
                {
                    new MovieRole
                    {
                        Artist = artist1,
                        CharacterPlayed = "character1",
                    },
                }
            };
            var movieReview = new MovieReview
            {
                User = user1,
                Movie = movie1,
                Content = "content",
                Rating = 9,
                Date = DateTime.Parse("26 July 2019"),
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieReviews.AddAsync(movieReview);
            await dbContext.SaveChangesAsync();

            var id = dbContext.Movies.First().Id;

            var reviewService = new Mock<IReviewService>();
            reviewService.Setup(r => r.ReviewExistsAsync("", ""))
                        .ReturnsAsync(false);
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new MovieDetailsViewModel
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = "genre1",
                Director = "name1",
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
                IsReviewedByCurrentUser = false,
                Rating = 9,
                ReviewsCount = 1,
                RandomReview = new MovieReviewViewModel
                {
                    User = "user1",
                    Movie = "movie1",
                    Content = "content",
                    Rating = 9,
                    Date = DateTime.Parse("26 July 2019"),
                },
            };

            var actualResult = await movieService.GetMovieAndDetailsByIdAsync(id);

            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.ReleaseDate, actualResult.ReleaseDate);
            Assert.Equal(expectedResult.Description, actualResult.Description);
            Assert.Equal(expectedResult.Length, actualResult.Length);
            Assert.Equal(expectedResult.Genre, actualResult.Genre);
            Assert.Equal(expectedResult.Director, actualResult.Director);
            Assert.Equal(expectedResult.CoverImageLink, actualResult.CoverImageLink);
            Assert.Equal(expectedResult.TrailerLink, actualResult.TrailerLink);

            Assert.Equal(expectedResult.Rating, actualResult.Rating);
            Assert.Equal(expectedResult.ReviewsCount, actualResult.ReviewsCount);

            Assert.Equal(expectedResult.RandomReview.User, actualResult.RandomReview.User);
            Assert.Equal(expectedResult.RandomReview.Movie, actualResult.RandomReview.Movie);
            Assert.Equal(expectedResult.RandomReview.Content, actualResult.RandomReview.Content);
            Assert.Equal(expectedResult.RandomReview.Rating, actualResult.RandomReview.Rating);
            Assert.Equal(expectedResult.RandomReview.Date, actualResult.RandomReview.Date);

            Assert.True(actualResult.Cast.Count() == 1);
            Assert.True(actualResult.Cast.First().Actor == "name1");
            Assert.True(actualResult.Cast.First().MovieCharacter == "character1");
        }

        [Theory]
        [InlineData("movie1", "genre1", "name1", false, 1)]
        [InlineData("movie2", "genre2", "name1", false, 2)]
        [InlineData("movie2", "genre1", "name2", false, 3)]
        [InlineData("movie2", "genre1", "name1", true, 4)]
        public async Task CreateMovieShouldReturnFalseIfProvidedGenreArtistOrMovieNameIsInvalid(string movieName, string genreName, string directorName, bool expectedResult, int n)
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"CreateMovie_Db_{n}")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
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
                Genre = genre1,
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);            
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new CreateMovieInputModel
            {
                Name = movieName,
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = genreName,
                Director = directorName,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };

            var actualResult = await movieService.CreateMovieAsync(input);

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task CreateMovieShouldSetDefaultCoverAndTrailerIfNotProvided()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateMovie_Db_5")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new CreateMovieInputModel
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = "genre1",
                Director = "name1",
                CoverImageLink = "",
                TrailerLink = "       ",
            };

            var actualResult = await movieService.CreateMovieAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Movies.Count() == 1);
            Assert.Equal(genre1, dbContext.Movies.First().Genre);
            Assert.Equal(artist1, dbContext.Movies.First().Director);
            Assert.Equal(GlobalConstants.noImageLink, dbContext.Movies.First().CoverImageLink);
            Assert.Equal(GlobalConstants.noTrailerLink, dbContext.Movies.First().TrailerLink);
        }

        [Theory]
        [InlineData("movie2", "name1", 1, false)]
        [InlineData("movie1", "name3", 2, false)]
        [InlineData("movie1", "name1", 3, true)]
        [InlineData("movie1", "name2", 4, false)]
        public async Task AddRoleToMovieShouldReturnFalseIfProvidedArtistOrMovieNameIsInvalid(string movieName, string artistName, int n, bool expectedResult)
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"AddRoletoMovie_Db_{n}")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var artist2 = new Artist
            {
                FullName = "name2",
                Biography = "biography2",
                BirthDate = DateTime.Parse("24 July 2019"),
                PhotoLink = "photo2",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = genre1,
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var movieRole = new MovieRole
            {
                Movie = movie1,
                Artist = artist2,
                CharacterPlayed = "something",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Artists.AddAsync(artist2);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieRoles.AddAsync(movieRole);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddMovieRoleInputModel
            {
                Artist = artistName,
                Movie = movieName,
                CharacterPlayed = "character1",
            };

            var actualResult = await movieService.AddRoleToMovieAsync(input);

            Assert.True(actualResult == expectedResult);
        }

        [Fact]
        public async Task AddRoleToMovieShouldAddRoleProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "AddRoletoMovie_Db_5")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
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
                Genre = genre1,
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddMovieRoleInputModel
            {
                Artist = "name1",
                Movie = "movie1",
                CharacterPlayed = "character1",
            };

            var actualResult = await movieService.AddRoleToMovieAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.MovieRoles.Count() == 1);
            Assert.Equal(movie1, dbContext.MovieRoles.First().Movie);
            Assert.Equal(artist1, dbContext.MovieRoles.First().Artist);
            Assert.Equal("character1", dbContext.MovieRoles.First().CharacterPlayed);
        }

        [Theory]
        [InlineData("genre2", "name1", 1, false)]
        [InlineData("genre1", "name2", 2, false)]
        [InlineData("genre1", "name1", 3, true)]
        public async Task UpdateMovieShouldReturnFalseIfProvidedArtistOrGenreIsInvalid(string genre, string artistName, int n, bool expectedResult)
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"UpdateMovie_Db_{n}")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
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
                Genre = genre1,
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateMovieInputModel
            {
                Id = movie1.Id,
                Name = "new name",
                ReleaseDate = DateTime.Parse("24 July 2003"),
                Description = "new desc",
                Length = 120,
                Genre = genre,
                Director = artistName,
                CoverImageLink = "new cover",
                TrailerLink = "new trailer",
            };

            var actualResult = await movieService.UpdateMovieAsync(input);

            Assert.True(actualResult == expectedResult);
        }

        [Fact]
        public async Task UpdateMovieShouldReturnFalseIfProvidedIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "UpdateMovie_Db_4")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
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
                Genre = genre1,
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateMovieInputModel
            {
                Id = "invalid id",
                Name = "new name",
                ReleaseDate = DateTime.Parse("24 July 2003"),
                Description = "new desc",
                Length = 120,
                Genre = "genre1",
                Director = "name1",
                CoverImageLink = "new cover",
                TrailerLink = "new trailer",
            };

            var actualResult = await movieService.UpdateMovieAsync(input);

            Assert.False(actualResult);
        }

        [Fact]
        public async Task UpdateMovieShouldUpdateProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "UpdateMovie_Db_5")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var genre1 = new Genre
            {
                Name = "genre1"
            };
            var genre2 = new Genre
            {
                Name = "genre2"
            };
            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var artist2 = new Artist
            {
                FullName = "name2",
                Biography = "biography2",
                BirthDate = DateTime.Parse("26 July 2019"),
                PhotoLink = "photo2",
            };
            var movie1 = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Length = 160,
                Genre = genre1,
                Director = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Genres.AddAsync(genre2);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Artists.AddAsync(artist2);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.SaveChangesAsync();

            var reviewService = new Mock<IReviewService>();
            var watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MoviesProfile());
            });
            var mapper = config.CreateMapper();
            var movieService = new MovieService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateMovieInputModel
            {
                Id = movie1.Id,
                Name = "new name",
                ReleaseDate = DateTime.Parse("24 July 2003"),
                Description = "new desc",
                Length = 120,
                Genre = "genre2",
                Director = "name2",
                CoverImageLink = "new cover",
                TrailerLink = "new trailer",
            };

            var actualResult = await movieService.UpdateMovieAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Movies.Count() == 1);

            Assert.Equal(120, dbContext.Movies.First().Length);
            Assert.Equal("new name", dbContext.Movies.First().Name);
            Assert.Equal("new desc", dbContext.Movies.First().Description);
            Assert.Equal(DateTime.Parse("24 July 2003"), dbContext.Movies.First().ReleaseDate);
            Assert.Equal(genre2, dbContext.Movies.First().Genre);
            Assert.Equal(artist2, dbContext.Movies.First().Director);
            Assert.Equal("new cover", dbContext.Movies.First().CoverImageLink);
            Assert.Equal("new trailer", dbContext.Movies.First().TrailerLink);
        }
    }
}
