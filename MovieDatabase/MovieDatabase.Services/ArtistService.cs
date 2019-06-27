using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services.Contracts;
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

        public List<ArtistAllViewModel> GetAllArtists()
        {
            var allArtists = this.dbContext.Artists
                .Select(a => new ArtistAllViewModel
                {
                    FullName = a.FullName,
                    PhotoLink = a.PhotoLink,
                    Biography = a.Biography,
                })
                .ToList();

            return allArtists;
        }
    }
}
