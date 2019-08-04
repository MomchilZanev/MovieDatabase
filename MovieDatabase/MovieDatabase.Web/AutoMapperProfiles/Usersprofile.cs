using AutoMapper;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.User;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<MovieDatabaseUser, UserNameViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(src => src.UserName));
        }
    }
}