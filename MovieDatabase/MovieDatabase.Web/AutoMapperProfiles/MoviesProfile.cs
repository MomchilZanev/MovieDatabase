using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Models.ViewModels.Movie;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Movie, MovieAllViewModel>()
                .ForMember(x => x.Genre, y => y.MapFrom(src => src.Genre.Name));

            CreateMap<Movie, MovieNameViewModel>();

            CreateMap<MovieRole, MovieCastViewModel>()
                .ForMember(x => x.Actor, y => y.MapFrom(src => src.Artist.FullName))
                .ForMember(x => x.MovieCharacter, y => y.MapFrom(src => src.CharacterPlayed));

            CreateMap<Movie, MovieDetailsViewModel>()
                .ForMember(x => x.Director, y => y.MapFrom(src => src.Director.FullName))
                .ForMember(x => x.Genre, y => y.MapFrom(src => src.Genre.Name))
                .ForMember(x => x.ReviewsCount, y => y.MapFrom(src => src.TotalReviews));

            CreateMap<CreateMovieInputModel, Movie>()
                .ForMember(x => x.Director, y => y.Ignore())
                .ForMember(x => x.Genre, y => y.Ignore())
                .ForMember(x => x.CoverImageLink, y => y.MapFrom(src => (string.IsNullOrEmpty(src.CoverImageLink) || string.IsNullOrWhiteSpace(src.CoverImageLink)) ? GlobalConstants.noImageLink : src.CoverImageLink))
                .ForMember(x => x.TrailerLink, y => y.MapFrom(src => (string.IsNullOrEmpty(src.TrailerLink) || string.IsNullOrWhiteSpace(src.TrailerLink)) ? GlobalConstants.noTrailerLink : src.TrailerLink.Replace("watch?v=", "embed/")));

            CreateMap<AddMovieRoleInputModel, MovieRole>()
                .ForMember(x => x.Artist, y => y.Ignore())
                .ForMember(x => x.Movie, y => y.Ignore());

            CreateMap<MovieDetailsViewModel, UpdateMovieInputModel>();
        }
    }
}