using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class TVShowsProfile : Profile
    {
        public TVShowsProfile()
        {
            CreateMap<TVShow, TVShowNameViewModel>();

            CreateMap<Season, SeasonsAndTVShowNameViewModel>()
                .ForMember(x => x.SeasonId, y => y.MapFrom(src => src.Id))
                .ForMember(x => x.TVShowName, y => y.MapFrom(src => src.TVShow.Name));

            CreateMap<TVShow, TVShowAllViewModel>()
                .ForMember(x => x.Genre, y => y.MapFrom(src => src.Genre.Name))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.OverallRating));

            CreateMap<TVShow, TVShowDetailsViewModel>()
                .ForMember(x => x.Creator, y => y.MapFrom(src => src.Creator.FullName))
                .ForMember(x => x.Genre, y => y.MapFrom(src => src.Genre.Name))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.OverallRating))
                .ForMember(x => x.Seasons, y => y.MapFrom(src => new Dictionary<string, int>()))
                .ForMember(x => x.Episodes, y => y.MapFrom(src => src.Seasons.Sum(s => s.Episodes)));

            CreateMap<Season, SeasonDetailsViewModel>()
                .ForMember(x => x.TVShowId, y => y.MapFrom(src => src.TVShow.Id))
                .ForMember(x => x.TVShow, y => y.MapFrom(src => src.TVShow.Name))
                .ForMember(x => x.ReviewsCount, y => y.MapFrom(src => src.TotalReviews))
                .ForMember(x => x.Cast, y => y.Ignore());

            CreateMap<SeasonRole, SeasonCastViewModel>()
                .ForMember(x => x.Actor, y => y.MapFrom(src => src.Artist.FullName))
                .ForMember(x => x.TVShowCharacter, y => y.MapFrom(src => src.CharacterPlayed));

            CreateMap<CreateTVShowInputModel, TVShow>()
                .ForMember(x => x.Creator, y => y.Ignore())
                .ForMember(x => x.Genre, y => y.Ignore())
                .ForMember(x => x.CoverImageLink, y => y.MapFrom(src => (string.IsNullOrEmpty(src.CoverImageLink) || string.IsNullOrWhiteSpace(src.CoverImageLink)) ? GlobalConstants.noImageLink : src.CoverImageLink))
                .ForMember(x => x.TrailerLink, y => y.MapFrom(src => (string.IsNullOrEmpty(src.TrailerLink) || string.IsNullOrWhiteSpace(src.TrailerLink)) ? GlobalConstants.noTrailerLink : src.TrailerLink));

            CreateMap<AddSeasonInputModel, Season>()
                .ForMember(x => x.TVShow, y => y.Ignore())
                .ForMember(x => x.SeasonNumber, y => y.Ignore());

            CreateMap<AddSeasonRoleInputModel, SeasonRole>()
                .ForMember(x => x.Season, y => y.Ignore())
                .ForMember(x => x.Artist, y => y.Ignore());

            CreateMap<TVShowDetailsViewModel, UpdateTVShowInputModel>();

            CreateMap<SeasonDetailsViewModel, UpdateSeasonInputModel>();
        }
    }
}