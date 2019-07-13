using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.TVShow;
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
        private readonly IReviewService reviewService;

        public TVShowService(MovieDatabaseDbContext dbContext, IReviewService reviewService)
        {
            this.dbContext = dbContext;
            this.reviewService = reviewService;
        }        

        //TODO: Implement AutoMapper
        public List<TVShowAllViewModel> GetAllTVShowsAndOrder(string orderBy = null, string genreFilter = null, string userId = null)
        {
            var allTVShows = dbContext.TVShows.ToList();

            if (dbContext.Genres.Any(g => g.Name == genreFilter))
            {
                allTVShows = allTVShows.Where(t => t.Genre.Name == genreFilter).ToList();
            }

            var tvShowAllViewModel = allTVShows
                .Select(t => new TVShowAllViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    CoverImageLink = t.CoverImageLink,
                    FirstAired = t.FirstAired,
                    Rating = t.OverallRating,
                    TotalReviews = t.Seasons.Sum(s => s.Reviews.Count()),
                    Watchlisted = dbContext.TVShowUsers.Any(tu => tu.TVShowId == t.Id && tu.UserId == userId),
                }).ToList();

            if (orderBy == "release")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(t => t.FirstAired != null)
                    .OrderByDescending(t => t.FirstAired)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(t => t.FirstAired != null)
                    .OrderByDescending(t => t.TotalReviews)
                    .ToList();
            }
            else if (orderBy == "rating")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(t => t.FirstAired != null)
                    .OrderByDescending(t => t.Rating)
                    .ToList();
            }
            else if (orderBy == "soon")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(m => m.FirstAired != null && m.FirstAired > DateTime.UtcNow)
                    .OrderBy(m => m.FirstAired)
                    .ToList();
            }

            return tvShowAllViewModel;
        }

        public TVShowDetailsViewModel GetTVShowAndDetailsById(string tvShowId, string userId)
        {
            var tvShow = dbContext.TVShows.Find(tvShowId);

            var tvShowDetailsViewModel = new TVShowDetailsViewModel
            {
                Id = tvShow.Id,
                Name = tvShow.Name,
                Creator = tvShow.Creator.FullName,
                CoverImageLink = tvShow.CoverImageLink,
                TrailerLink = tvShow.TrailerLink,
                Description = tvShow.Description,
                Genre = tvShow.Genre.Name,
                Rating = tvShow.OverallRating,
                FirstAired = tvShow.FirstAired,
                Seasons = new Dictionary<string, int>(),
                Episodes = tvShow.Seasons.Sum(s => s.Episodes),
            };

            if (tvShow.Seasons.Any())
            {
                foreach (var season in tvShow.Seasons.OrderBy(s => s.SeasonNumber))
                {
                    tvShowDetailsViewModel.Seasons.Add(season.Id, season.SeasonNumber);
                }
            }            

            return tvShowDetailsViewModel;
        }

        public SeasonDetailsViewModel GetSeasonAndDetailsById(string seasonId, string userId)
        {
            var season = dbContext.Seasons.Find(seasonId);

            var randomReview = new SeasonReviewViewModel();

            if (season.Reviews.Count > 0)
            {
                Random rnd = new Random();
                int reviewIndex = rnd.Next(0, season.Reviews.Count());

                var reviewFromDb = season.Reviews.ToList()[reviewIndex];

                randomReview.TVShow = season.TVShow.Name;
                randomReview.Season = season.SeasonNumber;
                randomReview.User = reviewFromDb.User.UserName;
                randomReview.Content = reviewFromDb.Content;
                randomReview.Rating = reviewFromDb.Rating;
                randomReview.Date = reviewFromDb.Date;
            }

            var seasonDetailsViewModel = new SeasonDetailsViewModel
            {
                Id = season.Id,
                TVShow = season.TVShow.Name,
                SeasonNumber = season.SeasonNumber,
                Episodes = season.Episodes,
                LengthPerEpisode = season.LengthPerEpisode,
                Rating = season.Rating,
                ReleaseDate = season.ReleaseDate,
                Cast = season.Cast.Select(actor => new SeasonCastViewModel
                {
                    Actor = actor.Artist.FullName,
                    TVShowCharacter = actor.CharacterPlayed,
                }).ToList(),
                RandomReview = randomReview,
                ReviewsCount = season.Reviews.Count(),
                IsReviewedByCurrentUser = reviewService.ReviewExists(userId, season.Id),
            };

            return seasonDetailsViewModel;
        }

        public bool CreateTVShow(CreateTVShowInputModel input)
        {
            if (!dbContext.Genres.Any(g => g.Name == input.Genre))
            {
                return false;
            }
            if (!dbContext.Artists.Any(a => a.FullName == input.Creator))
            {
                return false;
            }
            if (dbContext.TVShows.Any(m => m.Name == input.Name))
            {
                return false;
            }

            var genre = dbContext.Genres.SingleOrDefault(g => g.Name == input.Genre);
            var creator = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Creator);

            var tvShow = new TVShow
            {
                Name = input.Name,
                Genre = genre,
                Creator = creator,
                Description = input.Description,
                CoverImageLink = (input.CoverImageLink == "" || input.CoverImageLink == null) ? "/images/no_image.png" : input.CoverImageLink,
                TrailerLink = (input.TrailerLink == "" || input.TrailerLink == null) ? "https://www.youtube.com/embed/KAOdjqyG37A" : input.TrailerLink,
            };
            dbContext.TVShows.Add(tvShow);
            dbContext.SaveChanges();

            return true;
        }

        public bool AddSeasonToTVShow(AddSeasonInputModel input)
        {
            if (!dbContext.TVShows.Any(t => t.Name == input.TVShow))
            {
                return false;
            }

            var tvShow = dbContext.TVShows.SingleOrDefault(t => t.Name == input.TVShow);

            var season = new Season
            {
                TVShow = tvShow,
                SeasonNumber = tvShow.Seasons.Count() + 1,
                ReleaseDate = input.ReleaseDate,
                Episodes = input.Episodes,
                LengthPerEpisode = input.LengthPerEpisode,
            };
            dbContext.Seasons.Add(season);
            dbContext.SaveChanges();

            return true;
        }

        public List<SeasonsAndTVShowNameViewModel> GetAllSeasonsAndTVShowNames()
        {
            var tvShowsAndSeasonsViewModel = dbContext.Seasons.Select(s => new SeasonsAndTVShowNameViewModel
            {
                SeasonId = s.Id,
                SeasonNumber = s.SeasonNumber,
                TVShowName = s.TVShow.Name,
            }).ToList();

            return tvShowsAndSeasonsViewModel;
        }

        public bool AddRoleToTVShowSeason(AddRoleInputModel input)
        {
            if (!dbContext.Seasons.Any(s => s.Id == input.SeasonId))
            {
                return false;
            }
            if (!dbContext.Artists.Any(a => a.FullName == input.Artist))
            {
                return false;
            }

            var season = dbContext.Seasons.SingleOrDefault(s => s.Id == input.SeasonId);
            var artist = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Artist);

            if (dbContext.SeasonRoles.Any(sr => sr.ArtistId == artist.Id && sr.SeasonId == season.Id))
            {
                return false;
            }

            var seasonRole = new SeasonRole
            {
                Season = season,
                Artist = artist,
                CharacterPlayed = input.CharacterPlayed,
            };
            dbContext.SeasonRoles.Add(seasonRole);
            dbContext.SaveChanges();

            return true;
        }        
    }
}
