using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class GenreServiceTests
    {
        [Fact]
        public void GetAllGenreNamesShouldReturnEmptyListIfDbIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllGenreNames_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GenresProfile());
            });
            var mapper = config.CreateMapper();

            var expectedResult = new List<GenreAllViewModel>();

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = genreService.GetAllGenreNames();

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task GetAllGenreNamesShouldReturnGenresProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllGenreNames_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            await dbContext.Genres.AddRangeAsync(new List<Genre>
            {
                new Genre{ Name = "genre1" },
                new Genre{ Name = "genre2" },
                new Genre{ Name = "genre3" },
            });
            await dbContext.SaveChangesAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GenresProfile());
            });
            var mapper = config.CreateMapper();

            var expectedResult = new List<GenreAllViewModel>
            {
                new GenreAllViewModel { Name = "genre1" },
                new GenreAllViewModel { Name = "genre2" },
                new GenreAllViewModel { Name = "genre3" },
            };

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = genreService.GetAllGenreNames();

            Assert.True(actualResult.Count() == 3);
            Assert.True(actualResult[0].Name == "genre1");
            Assert.True(actualResult[1].Name == "genre2");
            Assert.True(actualResult[2].Name == "genre3");
        }

        [Fact]
        public async Task CreateGenreAsyncShouldAddGenreToDbIfInputIsValid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateGenre_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var input = new CreateGenreInputModel
            {
                Name = "genre1"
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GenresProfile());
            });
            var mapper = config.CreateMapper();

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = await genreService.CreateGenreAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Genres.Count() == 1);
            Assert.True(dbContext.Genres.First().Name == "genre1");
        }

        [Fact]
        public async Task CreateGenreAsyncShouldReturnFalseIfGenreWithSameNameExists()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateGenre_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);
            await dbContext.Genres.AddAsync(new Genre { Name = "genre1" });
            await dbContext.SaveChangesAsync();

            var input = new CreateGenreInputModel
            {
                Name = "genre1"
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GenresProfile());
            });
            var mapper = config.CreateMapper();

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = await genreService.CreateGenreAsync(input);

            Assert.False(actualResult);
            Assert.True(dbContext.Genres.Count() == 1);
        }
    }
}
