using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
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
    public class TVShowServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly Mock<IReviewService> reviewService;
        private readonly Mock<IWatchlistService> watchlistService;
        private readonly IMapper mapper;

        public TVShowServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);

            this.reviewService = new Mock<IReviewService>();
            this.watchlistService = new Mock<IWatchlistService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TVShowsProfile());
            });
            this.mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllTVShowNamesShouldReturnEmptyListIfDbIsEmpty()
        {
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<TVShowNameViewModel>();

            var actualResult = await tvShowService.GetAllTVShowNamesAsync();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllTVShowNamesShouldReturnAllTVShowNamesProperly()
        {
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
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var tvSHow2 = new TVShow
            {
                Name = "tvShow2",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShows.AddAsync(tvSHow2);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<TVShowNameViewModel>
            {
                new TVShowNameViewModel
                {
                    Name = "tvShow1",
                },
                new TVShowNameViewModel
                {
                    Name = "tvShow2",
                }
            };

            var actualResult = await tvShowService.GetAllTVShowNamesAsync();

            Assert.True(actualResult.Count() == 2);

            Assert.Equal(expectedResult[0].GetType(), actualResult[0].GetType());

            Assert.True(expectedResult[0].Name == actualResult[0].Name);
            Assert.True(expectedResult[1].Name == actualResult[1].Name);
        }

        [Fact]
        public async Task GetAllSeasonIdsSeasonNumbersAndTVShowNamesShouldReturnEmptyListIfDbIsEmpty()
        {
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<SeasonsAndTVShowNameViewModel>();

            var actualResult = await tvShowService.GetAllSeasonIdsSeasonNumbersAndTVShowNamesAsync();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllSeasonIdsSeasonNumbersAndTVShowNamesShouldReturnCorrectValues()
        {
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
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                TVShow = tvShow1,
                SeasonNumber = 1,
                ReleaseDate = DateTime.Parse("10 February 2004"),
            };
            var tvShow2 = new TVShow
            {
                Name = "tvShow2",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            var season2 = new Season
            {
                TVShow = tvShow2,
                SeasonNumber = 1,
                ReleaseDate = DateTime.Parse("12 February 2004"),
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShows.AddAsync(tvShow2);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.Seasons.AddAsync(season2);
            await dbContext.SaveChangesAsync();

            var season1Id = season1.Id;
            var season2Id = season2.Id;

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<SeasonsAndTVShowNameViewModel>
            {
                new SeasonsAndTVShowNameViewModel
                {
                    TVShowName = "tvShow1",
                    SeasonId = season1Id,
                    SeasonNumber = 1,
                },
                new SeasonsAndTVShowNameViewModel
                {
                    TVShowName = "tvShow2",
                    SeasonId = season2Id,
                    SeasonNumber = 1,
                }
            };

            var actualResult = await tvShowService.GetAllSeasonIdsSeasonNumbersAndTVShowNamesAsync();

            Assert.True(actualResult.Count() == 2);

            Assert.Equal(expectedResult[0].GetType(), actualResult[0].GetType());

            Assert.True(expectedResult[0].TVShowName == actualResult[0].TVShowName);
            Assert.True(expectedResult[1].SeasonId == actualResult[1].SeasonId);
            Assert.True(expectedResult[1].SeasonNumber == actualResult[1].SeasonNumber);
        }

        [Fact]
        public async Task GetAllTVShowsShouldReturnEmptyListIfDbIsEmpty()
        {
            watchlistService.Setup(w => w.TVShowIsInUserWatchlistAsync("", ""))
                        .ReturnsAsync(false);

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<TVShowAllViewModel>();

            var actualResult = await tvShowService.GetAllTVShowsAsync();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllTVShowsShouldReturnAllTVShowsProperly()
        {
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
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                TVShow = tvShow1,
                SeasonNumber = 1,
                ReleaseDate = DateTime.Parse("10 February 2004"),
            };
            var tvShow2 = new TVShow
            {
                Name = "tvShow2",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            var season2 = new Season
            {
                TVShow = tvShow2,
                SeasonNumber = 1,
                ReleaseDate = DateTime.Parse("12 February 2004"),
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShows.AddAsync(tvShow2);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.Seasons.AddAsync(season2);
            await dbContext.SaveChangesAsync();

            watchlistService.Setup(w => w.TVShowIsInUserWatchlistAsync("", ""))
                        .ReturnsAsync(false);

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvShow1",
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    FirstAired = DateTime.Parse("10 February 2004"),
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvShow2",
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    FirstAired = DateTime.Parse("12 February 2004"),
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },                                
            };

            var actualResult = await tvShowService.GetAllTVShowsAsync();

            Assert.True(actualResult.Count() == 2);

            Assert.Equal(expectedResult[0].GetType(), actualResult[0].GetType());

            Assert.True(expectedResult[0].Name == actualResult[0].Name);
            Assert.True(expectedResult[0].FirstAired == actualResult[0].FirstAired);
            Assert.True(expectedResult[0].Genre == actualResult[0].Genre);
            Assert.True(expectedResult[0].CoverImageLink == actualResult[0].CoverImageLink);
            Assert.True(expectedResult[0].Rating == actualResult[0].Rating);
            Assert.True(expectedResult[0].TotalReviews == actualResult[0].TotalReviews);
            Assert.True(expectedResult[0].Watchlisted == actualResult[0].Watchlisted);
            Assert.True(expectedResult[1].Name == actualResult[1].Name);
            Assert.True(expectedResult[1].FirstAired == actualResult[1].FirstAired);
            Assert.True(expectedResult[1].Genre == actualResult[1].Genre);
            Assert.True(expectedResult[1].CoverImageLink == actualResult[1].CoverImageLink);
            Assert.True(expectedResult[1].Rating == actualResult[1].Rating);
            Assert.True(expectedResult[1].TotalReviews == actualResult[1].TotalReviews);
            Assert.True(expectedResult[1].Watchlisted == actualResult[1].Watchlisted);
        }

        [Theory]
        [InlineData("genre1", "tvShow1", 1)]
        [InlineData("genre2", "tvShow2", 1)]
        [InlineData("genre3", null, 0)]
        public async Task FilterTVShowsByGenreShouldFilterTVShowsProperly(string genre, string expectedTVShow, int expectedCount)
        {
            await dbContext.Genres.AddRangeAsync(new[]
            {
                new Genre{ Name = "genre1" },
                new Genre{ Name = "genre2" },
                new Genre{ Name = "genre3" }
            });
            await dbContext.SaveChangesAsync();

            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvShow1",
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvShow2",
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
            };
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = await tvShowService.FilterTVShowsByGenreAsync(input, genre);

            Assert.True(actualResult.Count() == expectedCount);
            if (actualResult.Count > 0)
            {
                Assert.True(actualResult.First().Name == expectedTVShow);
            }
        }

        [Fact]
        public async Task FilterTVShowsByGenreShouldReturnInputIfGenreGivenIsNotInDb()
        {
            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvShow1",
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvShow2",
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 0,
                    TotalReviews = 0,
                    Watchlisted = false,
                },
            };
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = await tvShowService.FilterTVShowsByGenreAsync(input, "genre");

            Assert.True(actualResult.Count() == 2);
        }

        [Fact]
        public void OrderTVShowsShouldReturnTVShowsOrderedByNewest()
        {
            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvSHow1",
                    Genre = "genre1",
                    FirstAired = DateTime.Parse("25 July 2018"),
                    CoverImageLink = "cover1",
                    Rating = 8,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvSHow2",
                    Genre = "genre2",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover2",
                    Rating = 8,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
            };

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = tvShowService.OrderTVShows(input, GlobalConstants.moviesTvShowsOrderByRelease);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderTVShowsShouldReturnTVShowsOrderedByRating()
        {
            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvSHow1",
                    Genre = "genre1",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover1",
                    Rating = 9,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvSHow2",
                    Genre = "genre2",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover2",
                    Rating = 10,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
            };

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = tvShowService.OrderTVShows(input, GlobalConstants.moviesTvShowsOrderByRating);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderTVShowsShouldReturnTVShowsOrderedByPopularity()
        {
            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvSHow1",
                    Genre = "genre1",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover1",
                    Rating = 9,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvSHow2",
                    Genre = "genre2",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover2",
                    Rating = 9,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
            };

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = tvShowService.OrderTVShows(input, GlobalConstants.moviesTvShowsOrderByPopularity);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderTVShowsShouldReturnUnreleasedTVShowsOrderedByOldest()
        {
            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvShow3",
                    FirstAired = DateTime.Parse("25 July 2021"),
                    Genre = "genre3",
                    CoverImageLink = "cover3",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvShow2",
                    FirstAired = DateTime.Parse("24 July 2019"),
                    Genre = "genre2",
                    CoverImageLink = "cover2",
                    Rating = 1,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvShow1",
                    FirstAired = DateTime.Parse("25 July 2020"),
                    Genre = "genre1",
                    CoverImageLink = "cover1",
                    Rating = 1,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
            };

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = tvShowService.OrderTVShows(input, GlobalConstants.moviesTvShowsShowComingSoon);

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderTVShowsShouldReturnInputIfOrderByIsInvalid()
        {
            var input = new List<TVShowAllViewModel>
            {
                new TVShowAllViewModel
                {
                    Name = "tvSHow1",
                    Genre = "genre1",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover1",
                    Rating = 9,
                    TotalReviews = 1,
                    Watchlisted = false,
                },
                new TVShowAllViewModel
                {
                    Name = "tvSHow2",
                    Genre = "genre2",
                    FirstAired = DateTime.Parse("25 July 2019"),
                    CoverImageLink = "cover2",
                    Rating = 9,
                    TotalReviews = 2,
                    Watchlisted = false,
                },
            };

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var actualResult = tvShowService.OrderTVShows(input, "invalid order by");

            Assert.True(actualResult.Count() == 2);
            Assert.Equal(input.First(), actualResult.First());
            Assert.Equal(input.Last(), actualResult.Last());
        }

        [Fact]
        public async Task GetTVShowAndDetailsByIdAsyncShouldThrowExceptionIfDbIsEmpty()
        {
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => tvShowService.GetTVShowAndDetailsByIdAsync("id"));
        }

        [Fact]
        public async Task GetTVShowAndDetailsByIdAsyncShouldThrowExceptionIfIdIsInvalid()
        {
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
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var tvShow2 = new TVShow
            {
                Name = "tvShow2",
                Description = "description2",
                Genre = new Genre { Name = "genre2" },
                Creator = artist1,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShows.AddAsync(tvShow2);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => tvShowService.GetTVShowAndDetailsByIdAsync("invalid Id"));
        }

        [Fact]
        public async Task GetTVShowAndDetailsByIdShouldReturnCorrectModel()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 24,
                LengthPerEpisode = 47,
                TVShow = tvShow1,
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Reviews = new List<SeasonReview>
                {
                    new SeasonReview
                    {
                        User = user1,
                        Content = "content",
                        Rating = 9,
                        Date = DateTime.UtcNow,

                    },
                }
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var tvShowId = tvShow1.Id;
            var seasonId = season1.Id;

            reviewService.Setup(r => r.ReviewExistsAsync("", ""))
                        .ReturnsAsync(false);

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new TVShowDetailsViewModel
            {
                Id = tvShowId,
                Name = "tvShow1",
                FirstAired = DateTime.Parse("24 July 2019"),
                Description = "description1",
                Genre = "genre1",
                Creator = "name1",
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
                Rating = 9,
                Episodes = 24,
                Seasons = new Dictionary<string, int>(),
            };
            expectedResult.Seasons.Add(seasonId, 1);

            var actualResult = await tvShowService.GetTVShowAndDetailsByIdAsync(tvShowId);

            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.FirstAired, actualResult.FirstAired);
            Assert.Equal(expectedResult.Description, actualResult.Description);
            Assert.Equal(expectedResult.Genre, actualResult.Genre);
            Assert.Equal(expectedResult.Creator, actualResult.Creator);
            Assert.Equal(expectedResult.CoverImageLink, actualResult.CoverImageLink);
            Assert.Equal(expectedResult.TrailerLink, actualResult.TrailerLink);
            Assert.Equal(expectedResult.Rating, actualResult.Rating);
            Assert.Equal(expectedResult.Episodes, actualResult.Episodes);
            Assert.Equal(expectedResult.Seasons, actualResult.Seasons);
        }

        [Fact]
        public async Task GetSeasonAndDetailsByIdShouldThrowExceptionIfDbIsempty()
        {
            reviewService.Setup(r => r.ReviewExistsAsync("", ""))
                        .ReturnsAsync(false);

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => tvShowService.GetSeasonAndDetailsByIdAsync("Id"));
        }

        [Fact]
        public async Task GetSeasonAndDetailsByIdShouldThrowExceptionIfIdIsInvalid()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 25,
                TVShow = tvShow1,
                LengthPerEpisode = 57,
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Cast = new List<SeasonRole>
                {
                    new SeasonRole
                    {
                        Artist = artist1,
                        CharacterPlayed = "character1",
                    },
                }
            };
            var seasonReview = new SeasonReview
            {
                User = user1,
                Season = season1,
                Content = "content",
                Rating = 9,
                Date = DateTime.Parse("26 July 2019"),
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview);
            await dbContext.SaveChangesAsync();

            reviewService.Setup(r => r.ReviewExistsAsync("", ""))
                        .ReturnsAsync(false);

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => tvShowService.GetSeasonAndDetailsByIdAsync("invalid Id"));
        }

        [Fact]
        public async Task GetSeasonAndDetailsByIdShouldReturnCorrectModel()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 25,
                TVShow = tvShow1,
                LengthPerEpisode = 57,
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Cast = new List<SeasonRole>
                {
                    new SeasonRole
                    {
                        Artist = artist1,
                        CharacterPlayed = "character1",
                    },
                }
            };
            var seasonReview = new SeasonReview
            {
                User = user1,
                Season = season1,
                Content = "content",
                Rating = 9,
                Date = DateTime.Parse("26 July 2019"),
            };
            await dbContext.Users.AddAsync(user1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonReviews.AddAsync(seasonReview);
            await dbContext.SaveChangesAsync();

            var seasonId = season1.Id;

            reviewService.Setup(r => r.ReviewExistsAsync("", ""))
                        .ReturnsAsync(false);

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var expectedResult = new SeasonDetailsViewModel
            {
                TVShow = "tvShow1",
                SeasonNumber = 1,
                Episodes = 25,
                LengthPerEpisode = 57,
                ReleaseDate = DateTime.Parse("24 July 2019"),
                IsReviewedByCurrentUser = false,
                Rating = 9,
                ReviewsCount = 1,
                RandomReview = new SeasonReviewViewModel
                {
                    TVShow = "tvShow1",
                    User = "user1",
                    Season = 1,
                    Content = "content",
                    Rating = 9,
                    Date = DateTime.Parse("26 July 2019"),
                },
            };

            var actualResult = await tvShowService.GetSeasonAndDetailsByIdAsync(seasonId);

            Assert.Equal(expectedResult.TVShow, actualResult.TVShow);
            Assert.Equal(expectedResult.SeasonNumber, actualResult.SeasonNumber);
            Assert.Equal(expectedResult.ReleaseDate, actualResult.ReleaseDate);
            Assert.Equal(expectedResult.Episodes, actualResult.Episodes);
            Assert.Equal(expectedResult.LengthPerEpisode, actualResult.LengthPerEpisode);

            Assert.Equal(expectedResult.Rating, actualResult.Rating);
            Assert.Equal(expectedResult.ReviewsCount, actualResult.ReviewsCount);

            Assert.Equal(expectedResult.RandomReview.User, actualResult.RandomReview.User);
            Assert.Equal(expectedResult.RandomReview.TVShow, actualResult.RandomReview.TVShow);
            Assert.Equal(expectedResult.RandomReview.Season, actualResult.RandomReview.Season);
            Assert.Equal(expectedResult.RandomReview.Content, actualResult.RandomReview.Content);
            Assert.Equal(expectedResult.RandomReview.Rating, actualResult.RandomReview.Rating);
            Assert.Equal(expectedResult.RandomReview.Date, actualResult.RandomReview.Date);

            Assert.True(actualResult.Cast.Count() == 1);
            Assert.True(actualResult.Cast.First().Actor == "name1");
            Assert.True(actualResult.Cast.First().TVShowCharacter == "character1");
        }

        [Theory]
        [InlineData("tvShow1", "genre1", "name1", false)]
        [InlineData("tvShow2", "genre2", "name1", false)]
        [InlineData("tvShow2", "genre1", "name2", false)]
        [InlineData("tvShow2", "genre1", "name1", true)]
        public async Task CreateTVShowShouldReturnFalseIfProvidedGenreArtistOrTVShowNameIsInvalid(string tvShowName, string genreName, string creatorName, bool expectedResult)
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new CreateTVShowInputModel
            {
                Name = tvShowName,
                Description = "description1",
                Genre = genreName,
                Creator = creatorName,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };

            var actualResult = await tvShowService.CreateTVShowAsync(input);

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task CreateTVShowShouldSetDefaultCoverAndTrailerIfNotProvided()
        {
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

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new CreateTVShowInputModel
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = "genre1",
                Creator = "name1",
                CoverImageLink = "",
                TrailerLink = "       ",
            };

            var actualResult = await tvShowService.CreateTVShowAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.TVShows.Count() == 1);
            Assert.Equal(genre1, dbContext.TVShows.First().Genre);
            Assert.Equal(artist1, dbContext.TVShows.First().Creator);
            Assert.Equal(GlobalConstants.noImageLink, dbContext.TVShows.First().CoverImageLink);
            Assert.Equal(GlobalConstants.noTrailerLink, dbContext.TVShows.First().TrailerLink);
        }

        [Fact]
        public async Task AddSeasonToTVShowShouldReturnFalseIfTVShowDoesntExist()
        {
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddSeasonInputModel
            {
                TVShow = "tvShow1",
                Episodes = 21,
                LengthPerEpisode = 78,
                ReleaseDate = DateTime.Parse("25 July 2073"),
            };
            Assert.True(dbContext.Seasons.Count() == 0);

            var actualResult = await tvShowService.AddSeasonToTVShowAsync(input);

            Assert.False(actualResult);
            Assert.True(dbContext.Seasons.Count() == 0);
        }

        [Fact]
        public async Task AddSeasonToTVShowShouldReturnFalseIfTVShowNameIsInvalid()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddSeasonInputModel
            {
                TVShow = "invalid name",
                Episodes = 21,
                LengthPerEpisode = 78,
                ReleaseDate = DateTime.Parse("25 July 2073"),
            };
            Assert.True(dbContext.Seasons.Count() == 0);

            var actualResult = await tvShowService.AddSeasonToTVShowAsync(input);

            Assert.False(actualResult);
            Assert.True(dbContext.Seasons.Count() == 0);
        }

        [Fact]
        public async Task AddSeasonToTVShowShouldReturnTrueAndAddSeasonProperly()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddSeasonInputModel
            {
                TVShow = "tvShow1",
                Episodes = 21,
                LengthPerEpisode = 78,
                ReleaseDate = DateTime.Parse("25 July 2073"),
            };
            Assert.True(dbContext.Seasons.Count() == 0);

            var actualResult = await tvShowService.AddSeasonToTVShowAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Seasons.Count() == 1);

            Assert.Equal("tvShow1", dbContext.Seasons.First().TVShow.Name);
            Assert.Equal(DateTime.Parse("25 July 2073"), dbContext.Seasons.First().ReleaseDate);
            Assert.Equal(21, dbContext.Seasons.First().Episodes);
            Assert.Equal(78, dbContext.Seasons.First().LengthPerEpisode);
            Assert.Equal(1, dbContext.Seasons.First().SeasonNumber);
        }

        [Fact]
        public async Task AddRoleToTVShowSeasonShouldReturnFalseIfProvidedSeasonIdIsInvalid()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 20,
                LengthPerEpisode = 35,
                TVShow = tvShow1,
                ReleaseDate = DateTime.Parse("25 July 2022"),
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddSeasonRoleInputModel
            {
                Artist = "name1",
                SeasonId = "invalid id",
                CharacterPlayed = "character1",
            };

            var actualResult = await tvShowService.AddRoleToTVShowSeasonAsync(input);

            Assert.False(actualResult);
        }

        [Theory]
        [InlineData("name3", false)]
        [InlineData("name1", true)]
        [InlineData("name2", false)]
        public async Task AddRoleToTVShowSeasonShouldReturnFalseIfProvidedArtistIsInvalid(string artistName, bool expectedResult)
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 20,
                LengthPerEpisode = 35,
                TVShow = tvShow1,
                ReleaseDate = DateTime.Parse("25 July 2022"),
            };
            var seasonRole = new SeasonRole
            {
                Season = season1,
                Artist = artist2,
                CharacterPlayed = "something",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Artists.AddAsync(artist2);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SeasonRoles.AddAsync(seasonRole);
            await dbContext.SaveChangesAsync();

            var season1Id = season1.Id;

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddSeasonRoleInputModel
            {
                Artist = artistName,
                SeasonId = season1Id,
                CharacterPlayed = "character1",
            };

            var actualResult = await tvShowService.AddRoleToTVShowSeasonAsync(input);

            Assert.True(actualResult == expectedResult);
        }

        [Fact]
        public async Task AddRoleToTVShowSeasonShouldAddRoleProperly()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var season1 = new Season
            {
                SeasonNumber = 1,
                Episodes = 20,
                LengthPerEpisode = 35,
                TVShow = tvShow1,
                ReleaseDate = DateTime.Parse("25 July 2022"),
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.SaveChangesAsync();

            var season1Id = season1.Id;

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new AddSeasonRoleInputModel
            {
                Artist = "name1",
                SeasonId = season1Id,
                CharacterPlayed = "character1",
            };

            var actualResult = await tvShowService.AddRoleToTVShowSeasonAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.SeasonRoles.Count() == 1);
            Assert.Equal(season1, dbContext.SeasonRoles.First().Season);
            Assert.Equal(artist1, dbContext.SeasonRoles.First().Artist);
            Assert.Equal("character1", dbContext.SeasonRoles.First().CharacterPlayed);
        }

        [Theory]
        [InlineData("genre2", "name1", false)]
        [InlineData("genre1", "name2", false)]
        [InlineData("genre1", "name1", true)]
        public async Task UpdateTVShowShouldReturnFalseIfProvidedGenreOrArtistIsInvalid(string genreName, string creatorName, bool expectedResult)
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var id = tvShow1.Id;

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateTVShowInputModel
            {
                Id = id,
                Name = "tvShow2",
                Description = "description2",
                Genre = genreName,
                Creator = creatorName,
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };

            var actualResult = await tvShowService.UpdateTVShowAsync(input);

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task UpdateTVShowShouldReturnFalseIfProvidedIdIsInvalid()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateTVShowInputModel
            {
                Id = "invalid id",
                Name = "tvShow2",
                Description = "description2",
                Genre = "genre1",
                Creator = "name1",
                CoverImageLink = "cover2",
                TrailerLink = "trailer2",
            };

            var actualResult = await tvShowService.UpdateTVShowAsync(input);

            Assert.False(actualResult);
        }

        [Fact]
        public async Task UpdateTVShowShouldUpdateProperly()
        {
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
            var tvShow1 = new TVShow
            {
                Name = "tvShow1",
                Description = "description1",
                Genre = genre1,
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Genres.AddAsync(genre1);
            await dbContext.Genres.AddAsync(genre2);
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Artists.AddAsync(artist2);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateTVShowInputModel
            {
                Id = tvShow1.Id,
                Name = "new name",
                Description = "new desc",
                Genre = "genre2",
                Creator = "name2",
                CoverImageLink = "new cover",
                TrailerLink = "new trailer",
            };

            var actualResult = await tvShowService.UpdateTVShowAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.TVShows.Count() == 1);

            Assert.Equal("new name", dbContext.TVShows.First().Name);
            Assert.Equal("new desc", dbContext.TVShows.First().Description);
            Assert.Equal(genre2, dbContext.TVShows.First().Genre);
            Assert.Equal(artist2, dbContext.TVShows.First().Creator);
            Assert.Equal("new cover", dbContext.TVShows.First().CoverImageLink);
            Assert.Equal("new trailer", dbContext.TVShows.First().TrailerLink);
        }

        [Fact]
        public async Task UpdateSeasonShouldReturnFalseIfTVShowDoesntExist()
        {
            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateSeasonInputModel
            {
                Id = "id",
                TVShow = "tvShow1",
                Episodes = 21,
                LengthPerEpisode = 78,
                ReleaseDate = DateTime.Parse("25 July 2073"),
            };

            var actualResult = await tvShowService.UpdateSeasonAsync(input);

            Assert.False(actualResult);
        }

        [Fact]
        public async Task UpdateSeasonShouldReturnFalseIfSeasonDoesntExist()
        {
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
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.SaveChangesAsync();

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateSeasonInputModel
            {
                Id = "invalid id",
                TVShow = "tvShow1",
                Episodes = 21,
                LengthPerEpisode = 78,
                ReleaseDate = DateTime.Parse("25 July 2073"),
            };
            var actualResult = await tvShowService.UpdateSeasonAsync(input);

            Assert.False(actualResult);
        }

        [Fact]
        public async Task UpdateSeasonShouldReturnTrueAndUpdateSeasonProperly()
        {
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
                Description = "description1",
                Genre = new Genre { Name = "genre1" },
                Creator = artist1,
                CoverImageLink = "cover1",
                TrailerLink = "trailer1",
            };
            var tvShow2 = new TVShow
            {
                Name = "tvShow2",
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
                Episodes = 25,
                LengthPerEpisode = 35,
                ReleaseDate = DateTime.Parse("25 July 2030"),
            };
            var season2 = new Season
            {
                TVShow = tvShow2,
                SeasonNumber = 2,
                Episodes = 7,
                LengthPerEpisode = 100,
                ReleaseDate = DateTime.Parse("25 July 2025"),
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.TVShows.AddAsync(tvShow1);
            await dbContext.TVShows.AddAsync(tvShow2);
            await dbContext.Seasons.AddAsync(season1);
            await dbContext.Seasons.AddAsync(season2);
            await dbContext.SaveChangesAsync();

            var season1Id = season1.Id;

            var tvShowService = new TVShowService(dbContext, reviewService.Object, watchlistService.Object, mapper);

            var input = new UpdateSeasonInputModel
            {
                Id = season1Id,
                TVShow = "tvShow2",
                Episodes = 8,
                LengthPerEpisode = 85,
                ReleaseDate = DateTime.Parse("25 July 2031"),
            };

            var actualResult = await tvShowService.UpdateSeasonAsync(input);

            Assert.True(actualResult);
            Assert.True(tvShow1.Seasons.Count() == 0);
            Assert.True(tvShow2.Seasons.Count() == 2);

            Assert.Equal(tvShow2, season1.TVShow);
            Assert.Equal(2, season1.SeasonNumber);
            Assert.Equal(8, season1.Episodes);
            Assert.Equal(85, season1.LengthPerEpisode);
            Assert.Equal(DateTime.Parse("25 July 2031"), season1.ReleaseDate);
        }
    }
}