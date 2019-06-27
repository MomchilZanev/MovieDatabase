using MovieDatabase.Data;
using MovieDatabase.Models.ViewModels.TVShow;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class TVShowService : ITVShowService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public TVShowService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //TODO: Implement AutoMapper
        public List<TVShowAllViewModel> GetAllTVShowsAndOrder(string orderBy)
        {
            var allTVShows = dbContext.TVShows.ToList();

            var tvShowAllViewModel = allTVShows
                .Select(t => new TVShowAllViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    CoverImageLink = t.CoverImageLink,
                    FirstSeasonReleaseDate = t.Seasons.First().ReleaseDate,
                    Rating = t.OverallRating,
                    TotalReviews = t.Seasons.Sum(s => s.Reviews.Count())

                }).ToList();

            if (orderBy == "release")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(t => t.FirstSeasonReleaseDate != null)
                    .OrderByDescending(t => t.FirstSeasonReleaseDate)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(t => t.FirstSeasonReleaseDate != null)
                    .OrderByDescending(t => t.TotalReviews)
                    .ToList();
            }
            else if (orderBy == "rating")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(t => t.FirstSeasonReleaseDate != null)
                    .OrderByDescending(t => t.Rating)
                    .ToList();
            }
            else if (orderBy == "soon")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(m => m.FirstSeasonReleaseDate != null && m.FirstSeasonReleaseDate > DateTime.UtcNow)
                    .OrderBy(m => m.FirstSeasonReleaseDate)
                    .ToList();
            }

            return tvShowAllViewModel;
        }

        public TVShowDetailsViewModel GetTVShowAndDetailsById(string tvShowId)
        {
            var tvShow = dbContext.TVShows.Find(tvShowId);
            
            var tvShowDetailsViewModel = new TVShowDetailsViewModel
            {
                Id = tvShow.Id,
                Name = tvShow.Name,
                Creator = tvShow.Creator.FullName,
                CoverImageLink = tvShow.CoverImageLink,
                Description = tvShow.Description,
                Genre = tvShow.Genre.Name,
                Rating = tvShow.OverallRating,
                FirstSeasonReleaseDate = tvShow.Seasons.First().ReleaseDate,
                Seasons = tvShow.Seasons.Select(s => new SeasonDetailsViewModel
                {
                    Id = s.Id,
                    TVShow = tvShow.Name,
                    SeasonNumber = s.SeasonNumber,
                    Episodes = s.Episodes,
                    LengthPerEpisode = s.LengthPerEpisode,
                    Rating = s.Rating,
                    ReleaseDate = s.ReleaseDate,
                    Cast = s.Cast.Select(a => new SeasonCastViewModel
                    {
                        Actor = a.Artist.FullName,
                        TVShowCharacter = a.CharacterPlayed,
                    }).ToList(),
                    Reviews = s.Reviews.Select(r => new SeasonReviewViewModel
                    {
                        Season = s.SeasonNumber,
                        User = r.User.UserName,
                        Content = r.Content,
                        Date = r.Date,
                        Rating = r.Rating,
                    }).ToList(),
                }).ToList()
            };

            return tvShowDetailsViewModel;
        }
    }
}
