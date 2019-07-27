using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IGenreService
    {
        Task<List<GenreAllViewModel>> GetAllGenreNamesAsync();

        Task<bool> CreateGenreAsync(CreateGenreInputModel input);
    }
}
