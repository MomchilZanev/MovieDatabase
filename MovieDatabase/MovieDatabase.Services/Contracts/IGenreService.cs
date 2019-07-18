using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IGenreService
    {
        List<GenreAllViewModel> GetAllGenreNames();

        Task<bool> CreateGenreAsync(CreateGenreInputModel input);
    }
}
