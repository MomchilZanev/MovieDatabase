using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using System;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class AnnouncementsProfile : Profile
    {
        public AnnouncementsProfile()
        {
            CreateMap<Announcement, AnnouncementViewModel>();

            CreateMap<CreateAnnouncementInputModel, Announcement>()
                .ForMember(x => x.Date, y => y.MapFrom(src => DateTime.UtcNow))
                .ForMember(x => x.ImageLink, y => y.MapFrom(src => (string.IsNullOrEmpty(src.ImageLink) || string.IsNullOrWhiteSpace(src.ImageLink)) ? GlobalConstants.noImageLink : src.ImageLink));
        }
    }
}