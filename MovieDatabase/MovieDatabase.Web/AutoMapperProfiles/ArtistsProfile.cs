﻿using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class ArtistsProfile : Profile
    {
        public ArtistsProfile()
        {
            CreateMap<Artist, ArtistAllViewModel>()
                .ForMember(x => x.Biography, y => y.MapFrom(src => src.Biography.Substring(0, Math.Min(GlobalConstants.artistPreviewBiographyMaxCharLength, src.Biography.Length)) + GlobalConstants.fourDots))
                .ForMember(x => x.CareerProjects, y => y.MapFrom(src => src.MovieRoles.Count() + src.SeasonRoles.Count() + src.MoviesDirected.Count() + src.TVShowsCreated.Count()));

            CreateMap<Artist, ArtistNameViewModel>();

            CreateMap<Artist, ArtistFullBioViewModel>();

            CreateMap<Artist, ArtistDetailsViewModel>()
                .ForMember(x => x.MoviesDirected, y => y.MapFrom(src => src.MoviesDirected.Select(m => m.Name).ToList()))
                .ForMember(x => x.TVShowsCreated, y => y.MapFrom(src => src.TVShowsCreated.Select(t => t.Name).ToList()))
                .ForMember(x => x.MovieRoles, y => y.MapFrom(src => new Dictionary<string, string>()))
                .ForMember(x => x.SeasonRoles, y => y.MapFrom(src => new Dictionary<string, string>()));

            CreateMap<CreateArtistInputModel, Artist>()
                .ForMember(x => x.PhotoLink, y => y.MapFrom(src => string.IsNullOrEmpty(src.PhotoLink) ? GlobalConstants.noArtistImage : src.PhotoLink));

            CreateMap<ArtistDetailsViewModel, UpdateArtistInputModel>();
        }
    }
}