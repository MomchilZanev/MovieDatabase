using MovieDatabase.Models.InputModels.Genre;

namespace MovieDatabase.Services.Contracts
{
    public interface IGenreService
    {
        bool CreateGenre(CreateGenreInputModel input);
    }
}
