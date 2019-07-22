using AutoMapper;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Genre;
using MovieDatabase.Models.ViewModels.Genre;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class GenresProfile : Profile
    {
        public GenresProfile()
        {
            CreateMap<Genre, GenreAllViewModel>();

            CreateMap<CreateGenreInputModel, Genre>();
        }
    }
}