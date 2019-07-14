using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Artist;
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

        public ArtistFullBioViewModel GetArtistFullBioById(string artistId)
        {
            var artistFromDb = dbContext.Artists.Find(artistId);

            var artistDetailsViewModel = new ArtistFullBioViewModel
            {
                Id = artistFromDb.Id,
                FullName = artistFromDb.FullName,
                BirthDate = artistFromDb.BirthDate,
                Biography = artistFromDb.Biography,
                PhotoLink = artistFromDb.PhotoLink,
            };

            return artistDetailsViewModel;
        }

        public ArtistDetailsViewModel GetArtistAndDetailsById(string artistId)
        {
            var artistFromDb = dbContext.Artists.Find(artistId);

            var artistDetailsViewModel = new ArtistDetailsViewModel
            {
                Id = artistFromDb.Id,
                FullName = artistFromDb.FullName,
                BirthDate = artistFromDb.BirthDate,
                Biography = artistFromDb.Biography,
                PhotoLink = artistFromDb.PhotoLink,
                MoviesDirected = artistFromDb.MoviesDirected.Select(m => m.Name).ToList(),
                TVShowsCreated = artistFromDb.TVShowsCreated.Select(t => t.Name).ToList(),
                MovieRoles = new Dictionary<string, string>(),
                SeasonRoles = new Dictionary<string, string>(),
            };

            foreach (var movieRole in artistFromDb.MovieRoles)
            {
                artistDetailsViewModel.MovieRoles.Add(movieRole.Movie.Name, movieRole.CharacterPlayed);
            }

            foreach (var seasonRole in artistFromDb.SeasonRoles)
            {
                artistDetailsViewModel.SeasonRoles.Add(seasonRole.Season.TVShow.Name + " Season " + seasonRole.Season.SeasonNumber, seasonRole.CharacterPlayed);
            }

            return artistDetailsViewModel;
        }

        public List<ArtistAllViewModel> GetAllArtistsAndOrder(string orderBy = null)
        {
            var artistAllViewModel = dbContext.Artists
                .Select(artist => new ArtistAllViewModel
                {
                    Id = artist.Id,
                    FullName = artist.FullName,
                    PhotoLink = artist.PhotoLink,
                    Biography = artist.Biography.Substring(0, Math.Min(800, artist.Biography.Length)) + "....",
                    BirthDate = artist.BirthDate,
                    CareerProjects = artist.MovieRoles.Count() + artist.SeasonRoles.Count() + artist.MoviesDirected.Count() + artist.TVShowsCreated.Count(),
                })
                .ToList();

            if (orderBy == "youngest")
            {
                artistAllViewModel = artistAllViewModel
                    .OrderByDescending(artist => artist.BirthDate)
                    .ToList();
            }
            else if (orderBy == "oldest")
            {
                artistAllViewModel = artistAllViewModel
                    .OrderBy(artist => artist.BirthDate)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                artistAllViewModel = artistAllViewModel
                    .OrderByDescending(artist => artist.CareerProjects)
                    .ToList();
            }

            return artistAllViewModel;
        }

        public bool CreateArtist(CreateArtistInputModel input)
        {
            if (dbContext.Artists.Any(artist => artist.FullName == input.FullName && artist.Biography == input.Biography && artist.BirthDate == artist.BirthDate))
            {
                return false;
            }

            var artistForDb = new Artist
            {
                FullName = input.FullName,
                BirthDate = input.BirthDate,
                Biography = input.Biography,
                PhotoLink = (input.PhotoLink == "" || input.PhotoLink == null) ? "/images/no_artist_image.png" : input.PhotoLink,
            };

            dbContext.Artists.Add(artistForDb);
            dbContext.SaveChanges();

            return true;
        }
    }
}
