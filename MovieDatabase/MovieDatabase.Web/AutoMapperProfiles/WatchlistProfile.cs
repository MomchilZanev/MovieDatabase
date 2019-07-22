using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Watchlist;
using System;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class WatchlistProfile : Profile
    {
        public WatchlistProfile()
        {
            CreateMap<MovieUser, WatchlistAllViewModel>()
                .ForMember(x => x.Id, y => y.MapFrom(src => src.MovieId))
                .ForMember(x => x.Name, y => y.MapFrom(src => src.Movie.Name))
                .ForMember(x => x.Description, y => y.MapFrom(src => src.Movie.Description.Substring(0, Math.Min(GlobalConstants.movieTvShowPreviewDescriptionMaxCharLength, src.Movie.Description.Length)) + GlobalConstants.fourDots))
                .ForMember(x => x.CoverImageLink, y => y.MapFrom(src => src.Movie.CoverImageLink))
                .ForMember(x => x.ReleaseDate, y => y.MapFrom(src => src.Movie.ReleaseDate))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.Movie.Rating))
                .ForMember(x => x.Category, y => y.MapFrom(src => GlobalConstants.moviesCategory));

            CreateMap<TVShowUser, WatchlistAllViewModel>()
                .ForMember(x => x.Id, y => y.MapFrom(src => src.TVShowId))
                .ForMember(x => x.Name, y => y.MapFrom(src => src.TVShow.Name))
                .ForMember(x => x.Description, y => y.MapFrom(src => src.TVShow.Description.Substring(0, Math.Min(GlobalConstants.movieTvShowPreviewDescriptionMaxCharLength, src.TVShow.Description.Length)) + GlobalConstants.fourDots))
                .ForMember(x => x.CoverImageLink, y => y.MapFrom(src => src.TVShow.CoverImageLink))
                .ForMember(x => x.ReleaseDate, y => y.MapFrom(src => src.TVShow.FirstAired))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.TVShow.OverallRating))
                .ForMember(x => x.Category, y => y.MapFrom(src => GlobalConstants.tvShowsCategory));
        }
    }
}