using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public MovieService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Movie> GetAllMovies()
        {
            return this.dbContext.Movies.ToList();
        }
    }
}
