using AutoMapper;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class GenreService : IGenreService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public GenreService(MovieDatabaseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public List<GenreAllViewModel> GetAllGenreNames()
        {
            var genresFromDb = dbContext.Genres.ToList();

            var allGenreNames = mapper.Map<List<Genre>, List<GenreAllViewModel>>(genresFromDb);

            return allGenreNames;
        }

        public async Task<bool> CreateGenreAsync(CreateGenreInputModel input)
        {
            if (dbContext.Genres.Any(genre => genre.Name == input.Name))
            {
                return false;
            };

            var genreForDb = mapper.Map<CreateGenreInputModel, Genre>(input);

            await dbContext.Genres.AddAsync(genreForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
