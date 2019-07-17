﻿using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;
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

        public List<GenreAllViewModel> GetAllGenreNames()
        {
            var allGenreNames = dbContext.Genres.Select(genre => new GenreAllViewModel
            {
                Name = genre.Name,
            }).ToList();

            return allGenreNames;
        }

        public bool CreateGenre(CreateGenreInputModel input)
        {
            if (dbContext.Genres.Any(genre => genre.Name == input.Name))
            {
                return false;
            };

            var genreForDb = new Genre
            {
                Name = input.Name,
            };

            dbContext.Genres.Add(genreForDb);
            dbContext.SaveChanges();

            return true;
        }
    }
}
