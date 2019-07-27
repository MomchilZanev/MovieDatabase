using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class ArtistService : IArtistService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public ArtistService(MovieDatabaseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<ArtistAllViewModel>> GetAllArtistsAsync()
        {
            var artistsFromDb = await dbContext.Artists.ToListAsync();

            var artistsAllViewModel = mapper.Map<List<Artist>, List<ArtistAllViewModel>>(artistsFromDb);

            return artistsAllViewModel;
        }

        public async Task<List<ArtistNameViewModel>> GetAllArtistNamesAsync()
        {
            var artistsFromDb = await dbContext.Artists.ToListAsync();

            var allArtistNames = mapper.Map<List<Artist>, List<ArtistNameViewModel>>(artistsFromDb);

            return allArtistNames;
        }   

        public async Task<ArtistFullBioViewModel> GetArtistFullBioByIdAsync(string artistId)
        {
            var artistFromDb = await dbContext.Artists.FindAsync(artistId);

            var artistFullBioViewModel = mapper.Map<Artist, ArtistFullBioViewModel>(artistFromDb);

            return artistFullBioViewModel;
        }

        public async Task<ArtistDetailsViewModel> GetArtistAndDetailsByIdAsync(string artistId)
        {
            var artistFromDb = await dbContext.Artists.FindAsync(artistId);

            var artistDetailsViewModel = mapper.Map<Artist, ArtistDetailsViewModel>(artistFromDb);

            if (artistFromDb != null)
            {
                foreach (var movieRole in artistFromDb.MovieRoles)
                {
                    artistDetailsViewModel.MovieRoles.Add(movieRole.Movie.Name, movieRole.CharacterPlayed);
                }
                foreach (var seasonRole in artistFromDb.SeasonRoles)
                {
                    artistDetailsViewModel.SeasonRoles.Add(seasonRole.Season.TVShow.Name + GlobalConstants._Season_ + seasonRole.Season.SeasonNumber, seasonRole.CharacterPlayed);
                }
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
            if (await dbContext.Artists.AnyAsync(artist => artist.FullName == input.FullName && artist.Biography == input.Biography && artist.BirthDate == artist.BirthDate))
            {
                return false;
            }

            var artistForDb = mapper.Map<CreateArtistInputModel, Artist>(input);

            await dbContext.Artists.AddAsync(artistForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateArtistAsync(UpdateArtistInputModel input)
        {
            if (!await dbContext.Artists.AnyAsync(artist => artist.Id == input.Id))
            {
                return false;
            }

            var artistFromDb = await dbContext.Artists.FindAsync(input.Id);

            artistFromDb.FullName = input.FullName;
            artistFromDb.BirthDate = input.BirthDate;
            artistFromDb.Biography = input.Biography;
            artistFromDb.PhotoLink = (string.IsNullOrEmpty(input.PhotoLink) || string.IsNullOrWhiteSpace(input.PhotoLink)) ? GlobalConstants.noArtistImage : input.PhotoLink;

            dbContext.Update(artistFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
