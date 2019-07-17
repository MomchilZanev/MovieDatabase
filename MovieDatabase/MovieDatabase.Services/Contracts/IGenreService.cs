using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IGenreService
    {
        List<GenreAllViewModel> GetAllGenreNames();

        bool CreateGenre(CreateGenreInputModel input);
    }
}
