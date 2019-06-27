using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Artist;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IArtistService
    {
        List<ArtistAllViewModel> GetAllArtists();
    }
}
