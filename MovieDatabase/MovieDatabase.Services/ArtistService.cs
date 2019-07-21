using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class ArtistService : IArtistService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public ArtistService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<ArtistAllViewModel> GetAllArtists()
        {
            var artistsAllViewModel = dbContext.Artists
                .Select(artist => new ArtistAllViewModel
                {
                    Id = artist.Id,
                    FullName = artist.FullName,
                    PhotoLink = artist.PhotoLink,
                    Biography = artist.Biography.Substring(0, Math.Min(GlobalConstants.artistPreviewBiographyMaxCharLength, artist.Biography.Length)) + GlobalConstants.fourDots,
                    BirthDate = artist.BirthDate,
                    CareerProjects = artist.MovieRoles.Count() + artist.SeasonRoles.Count() + artist.MoviesDirected.Count() + artist.TVShowsCreated.Count(),
                })
                .ToList();


            return artistsAllViewModel;
        }

        public List<ArtistNameViewModel> GetAllArtistNames()
        {
            var allArtistNames = dbContext.Artists.Select(artist => new ArtistNameViewModel
            {
                FullName = artist.FullName,
            }).ToList();

            return allArtistNames;
        }

        public async Task<ArtistFullBioViewModel> GetArtistFullBioByIdAsync(string artistId)
        {
            var artistFromDb = await dbContext.Artists.FindAsync(artistId);

            var artistFullBioViewModel = new ArtistFullBioViewModel
            {
                Id = artistFromDb.Id,
                FullName = artistFromDb.FullName,
                BirthDate = artistFromDb.BirthDate,
                Biography = artistFromDb.Biography,
                PhotoLink = artistFromDb.PhotoLink,
            };

            return artistFullBioViewModel;
        }

        public async Task<ArtistDetailsViewModel> GetArtistAndDetailsByIdAsync(string artistId)
        {
            var artistFromDb = await dbContext.Artists.FindAsync(artistId);

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
                artistDetailsViewModel.SeasonRoles.Add(seasonRole.Season.TVShow.Name + GlobalConstants._Season_ + seasonRole.Season.SeasonNumber, seasonRole.CharacterPlayed);
            }

            return artistDetailsViewModel;
        }

        public List<ArtistAllViewModel> OrderArtists(List<ArtistAllViewModel> artistsAllViewModel, string orderBy)
        {
            switch (orderBy)
            {
                case GlobalConstants.artistsOrderByYoungest:
                    return artistsAllViewModel.OrderByDescending(artist => artist.BirthDate).ToList();
                case GlobalConstants.artistsOrderByOldest:
                    return artistsAllViewModel.OrderBy(artist => artist.BirthDate).ToList();
                case GlobalConstants.artistsOrderByMostPopular:
                    return artistsAllViewModel.OrderByDescending(artist => artist.CareerProjects).ToList();
                default:
                    return artistsAllViewModel.ToList();
            }
        }

        public async Task<bool> CreateArtistAsync(CreateArtistInputModel input)
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
                PhotoLink = string.IsNullOrEmpty(input.PhotoLink) ? GlobalConstants.noArtistImage : input.PhotoLink,
            };

            await dbContext.Artists.AddAsync(artistForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateArtistAsync(UpdateArtistInputModel input)
        {
            if (!dbContext.Artists.Any(artist => artist.Id == input.Id))
            {
                return false;
            }

            var artistFromDb = await dbContext.Artists.FindAsync(input.Id);

            artistFromDb.FullName = input.FullName;
            artistFromDb.BirthDate = input.BirthDate;
            artistFromDb.Biography = input.Biography;
            artistFromDb.PhotoLink = input.PhotoLink;

            dbContext.Update(artistFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
