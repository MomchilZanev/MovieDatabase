using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class GenreService : IGenreService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public GenreService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<string> GetAllGenres()
        {
            var genres = dbContext.Genres.Select(g => g.Name).ToList();

            return genres;
        }

        public bool CreateGenre(CreateGenreInputModel input)
        {
            if (dbContext.Genres.Any(g => g.Name == input.Name))
            {
                return false;
            };

            var genre = new Genre
            {
                Name = input.Name,
            };

            dbContext.Genres.Add(genre);
            dbContext.SaveChanges();

            return true;
        }
    }
}
