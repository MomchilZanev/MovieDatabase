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

        public List<ArtistAllViewModel> GetAllArtistsAndOrder(string orderBy = null)
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

        public bool CreateArtist(CreateArtistInputModel input)
        {
            if (dbContext.Artists.Any(a => a.FullName == input.FullName && a.Biography == input.Biography && a.BirthDate == a.BirthDate))
            {
                return false;
            }

            var artist = new Artist
            {
                FullName = input.FullName,
                BirthDate = input.BirthDate,
                Biography = input.Biography,
                PhotoLink = (input.PhotoLink == "" || input.PhotoLink == null) ? "/images/no_artist_image.png" : input.PhotoLink,
            };

            dbContext.Artists.Add(artist);
            dbContext.SaveChanges();

            return true;
        }
    }
}
