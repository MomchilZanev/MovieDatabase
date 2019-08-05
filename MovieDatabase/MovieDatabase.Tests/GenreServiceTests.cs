using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class GenreServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public GenreServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GenresProfile());
            });
            this.mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllGenreNamesShouldReturnEmptyListIfDbIsEmpty()
        {
            var expectedResult = new List<GenreAllViewModel>();

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = await genreService.GetAllGenreNamesAsync();

            Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public async Task GetAllGenreNamesShouldReturnGenresProperly()
        {
            await dbContext.Genres.AddRangeAsync(new List<Genre>
            {
                new Genre{ Name = "genre1" },
                new Genre{ Name = "genre2" },
                new Genre{ Name = "genre3" },
            });
            await dbContext.SaveChangesAsync();

            var expectedResult = new List<GenreAllViewModel>
            {
                new GenreAllViewModel { Name = "genre1" },
                new GenreAllViewModel { Name = "genre2" },
                new GenreAllViewModel { Name = "genre3" },
            };

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = await genreService.GetAllGenreNamesAsync();

            Assert.True(actualResult.Count() == 3);
            Assert.True(actualResult[0].Name == "genre1");
            Assert.True(actualResult[1].Name == "genre2");
            Assert.True(actualResult[2].Name == "genre3");
        }

        [Fact]
        public async Task CreateGenreAsyncShouldAddGenreToDbIfInputIsValid()
        {
            var input = new CreateGenreInputModel
            {
                Name = "genre1"
            };

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = await genreService.CreateGenreAsync(input);

            Assert.True(actualResult);
            Assert.True(dbContext.Genres.Count() == 1);
            Assert.True(dbContext.Genres.First().Name == "genre1");
        }

        [Fact]
        public async Task CreateGenreAsyncShouldReturnFalseIfGenreWithSameNameExists()
        {
            await dbContext.Genres.AddAsync(new Genre { Name = "genre1" });
            await dbContext.SaveChangesAsync();

            var input = new CreateGenreInputModel
            {
                Name = "genre1"
            };

            var genreService = new GenreService(dbContext, mapper);

            var actualResult = await genreService.CreateGenreAsync(input);

            Assert.False(actualResult);
            Assert.True(dbContext.Genres.Count() == 1);
        }
    }
}
