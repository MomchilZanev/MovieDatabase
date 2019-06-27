using MovieDatabase.Domain;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();

        Movie GetMovieById(string movieId);
    }
}
