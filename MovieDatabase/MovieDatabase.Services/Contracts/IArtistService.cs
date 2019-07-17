using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IArtistService
    {
        List<ArtistAllViewModel> GetAllArtists();

        List<ArtistNameViewModel> GetAllArtistNames();

        ArtistDetailsViewModel GetArtistAndDetailsById(string artistId);

        ArtistFullBioViewModel GetArtistFullBioById(string artistId);        

        List<ArtistAllViewModel> OrderArtists(List<ArtistAllViewModel> artistsAllViewModel, string orderBy);

        bool CreateArtist(CreateArtistInputModel input);
    }
}
