using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
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

        public async Task<List<GenreAllViewModel>> GetAllGenreNamesAsync()
        {
            var genresFromDb = await dbContext.Genres.ToListAsync();

            var allGenreNames = mapper.Map<List<Genre>, List<GenreAllViewModel>>(genresFromDb);

            return allGenreNames;
        }

        public async Task<bool> CreateGenreAsync(CreateGenreInputModel input)
        {
            if (await dbContext.Genres.AnyAsync(genre => genre.Name == input.Name))
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
