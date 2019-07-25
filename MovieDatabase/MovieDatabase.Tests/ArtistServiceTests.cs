using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class ArtistServiceTests
    {
        [Fact]
        public void GetAllArtistsShouldReturnEmptyListWithEmptyDb()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllArtists_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var expectedResult = new List<ArtistAllViewModel>();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.GetAllArtists();

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task GetAllAnnouncementsShouldReturnAllAnnouncementsProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllArtists_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            var movie = new Movie
            {
                Name = "movie1",
                ReleaseDate = DateTime.Parse("24 July 2019"),
                Description = "something",
                Length = 150,
                Genre = new Genre { Name = "Crime" },
                CoverImageLink = "image",
                TrailerLink = "trailer",
                Director = artist1,
            };
            var artist2 = new Artist
            {
                FullName = "name2",
                Biography = "biography2",
                BirthDate = DateTime.Parse("24 July 2019"),
                PhotoLink = "photo2"
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie);
            await dbContext.Artists.AddAsync(artist2);
            await dbContext.SaveChangesAsync();

            var expectedResult = new List<ArtistAllViewModel>()
            {
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1....",
                    BirthDate = DateTime.Parse("25 July 2019"),
                    PhotoLink = "photo1",
                    CareerProjects = 1,
                },
                new ArtistAllViewModel
                {
                    FullName = "name2",
                    Biography = "biography2....",
                    BirthDate = DateTime.Parse("24 July 2019"),
                    PhotoLink = "photo2",
                    CareerProjects = 0,
                }
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.GetAllArtists();

            Assert.True(actualResult.Count() == 2);
            for (int i = 0; i < expectedResult.Count(); i++)
            {
                Assert.Equal(expectedResult[i].FullName, actualResult[i].FullName);
                Assert.Equal(expectedResult[i].Biography, actualResult[i].Biography);
                Assert.Equal(expectedResult[i].BirthDate, actualResult[i].BirthDate);
                Assert.Equal(expectedResult[i].PhotoLink, actualResult[i].PhotoLink);
            }
        }

        [Fact]
        public void GetAllArtistNamesReturnEmptyListWithEmptyDb()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllArtistNames_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var expectedResult = new List<ArtistNameViewModel>();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.GetAllArtistNames();

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task GetAllArtistNamesShoulReturnNamesProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllArtistNames_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

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
                PhotoLink = "photo2"
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Artists.AddAsync(artist2);
            await dbContext.SaveChangesAsync();

            var expectedResult = new List<ArtistNameViewModel>()
            {
                new ArtistNameViewModel
                {
                    FullName = "name1",
                },
                new ArtistNameViewModel
                {
                    FullName = "name2",
                }
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.GetAllArtists();

            Assert.True(actualResult.Count() == 2);
            for (int i = 0; i < expectedResult.Count(); i++)
            {
                Assert.Equal(expectedResult[i].FullName, actualResult[i].FullName);
            }
        }

        [Fact]
        public async Task GetArtistFullBioByIdAsyncShouldReturnEmptyModelWithEmptyDb()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetArtistFullBioById_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.GetArtistFullBioByIdAsync("id");

            Assert.True(actualResult == null);
        }

        [Fact]
        public async Task GetArtistFullBioByIdAsyncShouldReturnEmptyIfIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetArtistFullBioById_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.SaveChangesAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.GetArtistFullBioByIdAsync("invalid");

            Assert.True(actualResult == null);
        }

        [Fact]
        public async Task GetArtistFullBioByIdAsyncShouldReturnCorrectModelIfDataIsValid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetArtistFullBioById_Db_3")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.SaveChangesAsync();

            var id = dbContext.Artists.First().Id;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.GetArtistFullBioByIdAsync(id);

            Assert.True(actualResult.FullName == "name1");
            Assert.True(actualResult.Biography == "biography1");
            Assert.True(actualResult.BirthDate == DateTime.Parse("25 July 2019"));
            Assert.True(actualResult.PhotoLink == "photo1");
        }

        [Fact]
        public async Task GetArtistAndDetailsByIdAsyncShouldReturnEmptyModelIfDbIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetArtistAndDetailsById_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.GetArtistAndDetailsByIdAsync("id");

            Assert.True(actualResult == null);
        }

        [Fact]
        public async Task GetArtistAndDetailsByIdAsyncShouldReturnEmptyModelIfIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetArtistAndDetailsById_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist1 = new Artist
            {
                FullName = "name1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.SaveChangesAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.GetArtistAndDetailsByIdAsync("invalid");

            Assert.True(actualResult == null);
        }

        [Fact]
        public async Task GetArtistAndDetailsByIdAsyncShouldReturnCorrectModelIfDataIsValid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetArtistAndDetailsById_Db_3")
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
                Description = "something",
                Length = 150,
                Genre = new Genre { Name = "Crime" },
                CoverImageLink = "image",
                TrailerLink = "trailer",
                Director = artist1,
            };
            var movieRole1 = new MovieRole
            {
                Movie = movie1,
                Artist = artist1,
                CharacterPlayed = "character",
            };
            await dbContext.Artists.AddAsync(artist1);
            await dbContext.Movies.AddAsync(movie1);
            await dbContext.MovieRoles.AddAsync(movieRole1);
            await dbContext.SaveChangesAsync();

            var id = dbContext.Artists.First().Id;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();

            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.GetArtistAndDetailsByIdAsync(id);

            Assert.True(actualResult.FullName == "name1");
            Assert.True(actualResult.Biography == "biography1");
            Assert.True(actualResult.BirthDate == DateTime.Parse("25 July 2019"));
            Assert.True(actualResult.PhotoLink == "photo1");
            Assert.True(actualResult.SeasonRoles.Count() == 0);
            Assert.True(actualResult.TVShowsCreated.Count() == 0);
            Assert.True(actualResult.MovieRoles.Count() == 1);
            Assert.True(actualResult.MoviesDirected.Count() == 1);
            Assert.True(actualResult.MovieRoles.First().Key == "movie1" && actualResult.MovieRoles.First().Value == "character");
            Assert.True(actualResult.MoviesDirected.First() == "movie1");
        }

        [Fact]
        public void OrderArtistsShouldReturnArtistsOrderedByYoungest()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderArtists_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<ArtistAllViewModel>()
            {
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2018"),
                    PhotoLink = "photo1",
                    CareerProjects = 1,
                },
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2019"),
                    PhotoLink = "photo1",
                    CareerProjects = 2,
                },                
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.OrderArtists(input, GlobalConstants.artistsOrderByYoungest);

            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderArtistsShouldReturnArtistsOrderedByOldest()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderArtists_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<ArtistAllViewModel>()
            {
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2019"),
                    PhotoLink = "photo1",
                    CareerProjects = 2,
                },
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2018"),
                    PhotoLink = "photo1",
                    CareerProjects = 1,
                },                
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.OrderArtists(input, GlobalConstants.artistsOrderByOldest);

            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderArtistsShouldReturnArtistsOrderedByMostPopular()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderArtists_Db_3")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<ArtistAllViewModel>()
            {
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2018"),
                    PhotoLink = "photo1",
                    CareerProjects = 1,
                },
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2019"),
                    PhotoLink = "photo1",
                    CareerProjects = 2,
                },                
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.OrderArtists(input, GlobalConstants.artistsOrderByMostPopular);

            Assert.Equal(input.First(), actualResult.Last());
            Assert.Equal(input.Last(), actualResult.First());
        }

        [Fact]
        public void OrderArtistsShouldReturnInputIfOrderByIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderArtists_Db_4")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new List<ArtistAllViewModel>()
            {
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2018"),
                    PhotoLink = "photo1",
                    CareerProjects = 1,
                },
                new ArtistAllViewModel
                {
                    FullName = "name1",
                    Biography = "biography1",
                    BirthDate = DateTime.Parse("25 July 2019"),
                    PhotoLink = "photo1",
                    CareerProjects = 2,
                },
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = artistService.OrderArtists(input, "something invalid");

            Assert.Equal(input.First(), actualResult.First());
            Assert.Equal(input.Last(), actualResult.Last());
        }

        [Fact]
        public async Task CreateArtistAsyncShouldAddArtistToDbIfInputIsValid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateArtist_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new CreateArtistInputModel
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.CreateArtistAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Artists.Count() == 1);
            Assert.True(dbContext.Artists.First().FullName == "artist1");
            Assert.True(dbContext.Artists.First().Biography == "biography1");
            Assert.True(dbContext.Artists.First().BirthDate == DateTime.Parse("25 July 2019"));
            Assert.True(dbContext.Artists.First().PhotoLink == "photo1");
        }

        [Fact]
        public async Task CreateArtistAsyncShouldReturnFalseIfArtistWithSameNameBiographyAndBDateAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateArtist_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new CreateArtistInputModel
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };

            await dbContext.Artists.AddAsync(new Artist
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            });
            await dbContext.SaveChangesAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.CreateArtistAsync(input);

            Assert.False(actualResult);
            Assert.True(dbContext.Artists.Count() == 1);
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData("", 4)]
        [InlineData("     ", 5)]
        public async Task CreateArtistAsyncShouldSetPhotoLinkIfNoneIsProvided(string photoLink, int n)
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"CreateArtist_Db_{n}")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new CreateArtistInputModel
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = photoLink,
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.CreateArtistAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Artists.Count() == 1);
            Assert.True(dbContext.Artists.First().PhotoLink == GlobalConstants.noArtistImage);
        }

        [Fact]
        public async Task UpdateArtistAsyncShouldUpdateArtistProperlyIfValidDataIsEntered()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"UpdateArtist_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist = new Artist
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Artists.AddAsync(artist);
            await dbContext.SaveChangesAsync();

            var input = new UpdateArtistInputModel
            {
                Id = dbContext.Artists.First().Id,
                FullName = "artist2",
                Biography = "biography2",
                BirthDate = DateTime.Parse("24 July 2019"),
                PhotoLink = "photo2",
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.UpdateArtistAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Artists.Count() == 1);
            Assert.True(dbContext.Artists.First().FullName == "artist2");
            Assert.True(dbContext.Artists.First().Biography == "biography2");
            Assert.True(dbContext.Artists.First().BirthDate == DateTime.Parse("24 July 2019"));
            Assert.True(dbContext.Artists.First().PhotoLink == "photo2");
        }

        [Fact]
        public async Task UpdateArtistAsyncShouldReturnFalseIfIdIsInvalidOrDbIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"UpdateArtist_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist = new Artist
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Artists.AddAsync(artist);
            await dbContext.SaveChangesAsync();

            var input = new UpdateArtistInputModel
            {
                Id = "invalid",
                FullName = "artist2",
                Biography = "biography2",
                BirthDate = DateTime.Parse("24 July 2019"),
                PhotoLink = "photo2",
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.UpdateArtistAsync(input);

            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData("", 4)]
        [InlineData("     ", 5)]
        public async Task UpdateArtistAsyncShouldSwitchPhotoLinkToDefaultImgaeIfPreviousIsRemoved(string photoLink, int n)
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: $"UpdateArtist_Db_{n}")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var artist = new Artist
            {
                FullName = "artist1",
                Biography = "biography1",
                BirthDate = DateTime.Parse("25 July 2019"),
                PhotoLink = "photo1",
            };
            await dbContext.Artists.AddAsync(artist);
            await dbContext.SaveChangesAsync();

            var input = new UpdateArtistInputModel
            {
                Id = dbContext.Artists.First().Id,
                FullName = "artist2",
                Biography = "biography2",
                BirthDate = DateTime.Parse("24 July 2019"),
                PhotoLink = photoLink,
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArtistsProfile());
            });
            var mapper = config.CreateMapper();
            var artistService = new ArtistService(dbContext, mapper);

            var actualResult = await artistService.UpdateArtistAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Artists.Count() == 1);
            Assert.True(dbContext.Artists.First().FullName == "artist2");
            Assert.True(dbContext.Artists.First().Biography == "biography2");
            Assert.True(dbContext.Artists.First().BirthDate == DateTime.Parse("24 July 2019"));
            Assert.True(dbContext.Artists.First().PhotoLink == GlobalConstants.noArtistImage);
        }
    }
}
