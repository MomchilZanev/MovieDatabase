﻿using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IArtistService
    {
        Task<List<ArtistAllViewModel>> GetAllArtistsAsync();

        Task<List<ArtistNameViewModel>> GetAllArtistNamesAsync();

        Task<ArtistDetailsViewModel> GetArtistAndDetailsByIdAsync(string artistId);

        Task<ArtistFullBioViewModel> GetArtistFullBioByIdAsync(string artistId);        

        List<ArtistAllViewModel> OrderArtists(List<ArtistAllViewModel> artistsAllViewModel, string orderBy);

        Task<bool> CreateArtistAsync(CreateArtistInputModel input);

        Task<bool> UpdateArtistAsync(UpdateArtistInputModel input);
    }
}
