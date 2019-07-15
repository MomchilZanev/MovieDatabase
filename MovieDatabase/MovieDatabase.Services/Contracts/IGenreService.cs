using MovieDatabase.Models.InputModels.Genre;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IGenreService
    {
        List<string> GetAllGenreNames();

        bool CreateGenre(CreateGenreInputModel input);
    }
}
