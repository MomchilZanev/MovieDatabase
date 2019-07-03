using MovieDatabase.Data;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class ArtistService : IArtistService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public ArtistService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ArtistDetailsViewModel GetArtistAndDetailsById(string artistId)
        {
            var artist = dbContext.Artists.Find(artistId);

            var artistDetailsViewModel = new ArtistDetailsViewModel
            {
                Id = artist.Id,
                FullName = artist.FullName,
                BirthDate = artist.BirthDate,
                Biography = artist.Biography,
                PhotoLink = artist.PhotoLink,
                MoviesDirected = artist.MoviesDirected.Select(m => m.Name).ToList(),
                TVShowsCreated = artist.TVShowsCreated.Select(t => t.Name).ToList(),
                MovieRoles = new Dictionary<string, string>(),
                SeasonRoles = new Dictionary<string, string>(),
            };

            foreach (var movieRole in artist.MovieRoles)
            {
                artistDetailsViewModel.MovieRoles.Add(movieRole.Movie.Name, movieRole.CharacterPlayed);
            }

            foreach (var seasonRole in artist.SeasonRoles)
            {
                artistDetailsViewModel.SeasonRoles.Add(seasonRole.Season.TVShow.Name + " Season " + seasonRole.Season.SeasonNumber, seasonRole.CharacterPlayed);
            }

            return artistDetailsViewModel;
        }

        public List<ArtistAllViewModel> GetAllArtistsAndOrder(string orderBy)
        {
            var artistAllViewModel = dbContext.Artists
                .Select(a => new ArtistAllViewModel
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    PhotoLink = a.PhotoLink,
                    Biography = a.Biography.Substring(0, Math.Min(260, a.Biography.Length)) + "....",
                    BirthDate = a.BirthDate,
                    CareerProjects = a.MovieRoles.Count() + a.SeasonRoles.Count() + a.MoviesDirected.Count() + a.TVShowsCreated.Count(),
                })
                .ToList();

            if (orderBy == "youngest")
            {
                artistAllViewModel = artistAllViewModel
                    .OrderByDescending(a => a.BirthDate)
                    .ToList();
            }
            else if (orderBy == "oldest")
            {
                artistAllViewModel = artistAllViewModel
                    .OrderBy(a => a.BirthDate)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                artistAllViewModel = artistAllViewModel
                    .OrderByDescending(a => a.CareerProjects)
                    .ToList();
            }

            return artistAllViewModel;
        }
    }
}
