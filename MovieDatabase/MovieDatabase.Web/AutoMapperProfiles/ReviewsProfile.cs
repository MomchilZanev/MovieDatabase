using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Review;
using MovieDatabase.Models.ViewModels.Review;
using System;

namespace MovieDatabase.Web.AutoMapperProfiles
{
    public class ReviewsProfile : Profile
    {
        public ReviewsProfile()
        {
            CreateMap<MovieReview, ReviewAllViewModel>()
                .ForMember(x => x.User, y => y.MapFrom(src => src.User.UserName))
                .ForMember(x => x.ItemId, y => y.MapFrom(src => src.MovieId))
                .ForMember(x => x.Item, y => y.MapFrom(src => src.Movie.Name));

            CreateMap<SeasonReview, ReviewAllViewModel>()
                .ForMember(x => x.User, y => y.MapFrom(src => src.User.UserName))
                .ForMember(x => x.ItemId, y => y.MapFrom(src => src.SeasonId))
                .ForMember(x => x.Item, y => y.MapFrom(src => src.Season.TVShow.Name + GlobalConstants._Season_ + src.Season.SeasonNumber));

            CreateMap<MovieReview, CreateReviewInputModel>()
                .ForMember(x => x.Id, y => y.MapFrom(src => src.MovieId));

            CreateMap<SeasonReview, CreateReviewInputModel>()
                .ForMember(x => x.Id, y => y.MapFrom(src => src.SeasonId));

            CreateMap<CreateReviewInputModel, MovieReview>()
                .ForMember(x => x.MovieId, y => y.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, y => y.Ignore())
                .ForMember(x => x.Date, y => y.MapFrom(src => DateTime.UtcNow));

            CreateMap<CreateReviewInputModel, SeasonReview>()
                .ForMember(x => x.SeasonId, y => y.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, y => y.Ignore())
                .ForMember(x => x.Date, y => y.MapFrom(src => DateTime.UtcNow));
        }
    }
}