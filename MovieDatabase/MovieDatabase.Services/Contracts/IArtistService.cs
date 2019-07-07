using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IArtistService
    {
        ArtistDetailsViewModel GetArtistAndDetailsById(string artistId);

        List<ArtistAllViewModel> GetAllArtistsAndOrder(string orderBy = null);

        bool CreateArtist(CreateArtistInputModel input);
    }
}
