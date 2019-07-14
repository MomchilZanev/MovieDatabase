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

        public List<TVShowAllViewModel> GetAllTVShowsAndOrder(string orderBy = null, string genreFilter = null, string userId = null)
        {
            var allTVShowsFromDb = dbContext.TVShows.ToList();

            if (dbContext.Genres.Any(genre => genre.Name == genreFilter))
            {
                allTVShowsFromDb = allTVShowsFromDb.Where(tvShow => tvShow.Genre.Name == genreFilter).ToList();
            }

            var tvShowAllViewModel = allTVShowsFromDb
                .Select(tvShow => new TVShowAllViewModel
                {
                    Id = tvShow.Id,
                    Name = tvShow.Name,
                    Description = tvShow.Description,
                    CoverImageLink = tvShow.CoverImageLink,
                    FirstAired = tvShow.FirstAired,
                    Rating = tvShow.OverallRating,
                    TotalReviews = tvShow.Seasons.Sum(season => season.Reviews.Count()),
                    Watchlisted = dbContext.TVShowUsers.Any(tvShowUser => tvShowUser.TVShowId == tvShow.Id && tvShowUser.UserId == userId),
                }).ToList();

            if (orderBy == "release")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(tvShow => tvShow.FirstAired != null)
                    .OrderByDescending(tvShow => tvShow.FirstAired)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(tvShow => tvShow.FirstAired != null)
                    .OrderByDescending(tvShow => tvShow.TotalReviews)
                    .ToList();
            }
            else if (orderBy == "rating")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(tvShow => tvShow.FirstAired != null)
                    .OrderByDescending(tvShow => tvShow.Rating)
                    .ToList();
            }
            else if (orderBy == "soon")
            {
                tvShowAllViewModel = tvShowAllViewModel
                    .Where(tvShow => tvShow.FirstAired != null && tvShow.FirstAired > DateTime.UtcNow)
                    .OrderBy(tvShow => tvShow.FirstAired)
                    .ToList();
            }

            return tvShowAllViewModel;
        }

        public TVShowDetailsViewModel GetTVShowAndDetailsById(string tvShowId, string userId)
        {
            var tvShowFromDb = dbContext.TVShows.Find(tvShowId);

            var tvShowDetailsViewModel = new TVShowDetailsViewModel
            {
                Id = tvShowFromDb.Id,
                Name = tvShowFromDb.Name,
                Creator = tvShowFromDb.Creator.FullName,
                CoverImageLink = tvShowFromDb.CoverImageLink,
                TrailerLink = tvShowFromDb.TrailerLink,
                Description = tvShowFromDb.Description,
                Genre = tvShowFromDb.Genre.Name,
                Rating = tvShowFromDb.OverallRating,
                FirstAired = tvShowFromDb.FirstAired,
                Seasons = new Dictionary<string, int>(),
                Episodes = tvShowFromDb.Seasons.Sum(s => s.Episodes),
            };

            if (tvShowFromDb.Seasons.Any())
            {
                foreach (var season in tvShowFromDb.Seasons.OrderBy(s => s.SeasonNumber))
                {
                    tvShowDetailsViewModel.Seasons.Add(season.Id, season.SeasonNumber);
                }
            }            

            return tvShowDetailsViewModel;
        }

        public SeasonDetailsViewModel GetSeasonAndDetailsById(string seasonId, string userId)
        {
            var seasonFromDb = dbContext.Seasons.Find(seasonId);

            var randomReview = new SeasonReviewViewModel();

            if (seasonFromDb.Reviews.Count > 0)
            {
                Random rnd = new Random();
                int reviewIndex = rnd.Next(0, seasonFromDb.Reviews.Count());

                var reviewFromDb = seasonFromDb.Reviews.ToList()[reviewIndex];

                randomReview.TVShow = seasonFromDb.TVShow.Name;
                randomReview.Season = seasonFromDb.SeasonNumber;
                randomReview.User = reviewFromDb.User.UserName;
                randomReview.Content = reviewFromDb.Content;
                randomReview.Rating = reviewFromDb.Rating;
                randomReview.Date = reviewFromDb.Date;
            }

            var seasonDetailsViewModel = new SeasonDetailsViewModel
            {
                Id = seasonFromDb.Id,
                TVShow = seasonFromDb.TVShow.Name,
                SeasonNumber = seasonFromDb.SeasonNumber,
                Episodes = seasonFromDb.Episodes,
                LengthPerEpisode = seasonFromDb.LengthPerEpisode,
                Rating = seasonFromDb.Rating,
                ReleaseDate = seasonFromDb.ReleaseDate,
                Cast = seasonFromDb.Cast.Select(actor => new SeasonCastViewModel
                {
                    Actor = actor.Artist.FullName,
                    TVShowCharacter = actor.CharacterPlayed,
                }).ToList(),
                RandomReview = randomReview,
                ReviewsCount = seasonFromDb.Reviews.Count(),
                IsReviewedByCurrentUser = reviewService.ReviewExists(userId, seasonFromDb.Id),
            };

            return seasonDetailsViewModel;
        }

        public bool CreateTVShow(CreateTVShowInputModel input)
        {
            if (!dbContext.Genres.Any(genre => genre.Name == input.Genre))
            {
                return false;
            }
            if (!dbContext.Artists.Any(artist => artist.FullName == input.Creator))
            {
                return false;
            }
            if (dbContext.TVShows.Any(tvShow => tvShow.Name == input.Name))
            {
                return false;
            }

            var genreFromDb = dbContext.Genres.SingleOrDefault(g => g.Name == input.Genre);
            var creatorFromDb = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Creator);

            var tvShowForDb = new TVShow
            {
                Name = input.Name,
                Genre = genreFromDb,
                Creator = creatorFromDb,
                Description = input.Description,
                CoverImageLink = (input.CoverImageLink == "" || input.CoverImageLink == null) ? "/images/no_image.png" : input.CoverImageLink,
                TrailerLink = (input.TrailerLink == "" || input.TrailerLink == null) ? "https://www.youtube.com/embed/KAOdjqyG37A" : input.TrailerLink,
            };

            dbContext.TVShows.Add(tvShowForDb);
            dbContext.SaveChanges();

            return true;
        }

        public bool AddSeasonToTVShow(AddSeasonInputModel input)
        {
            if (!dbContext.TVShows.Any(tvShow => tvShow.Name == input.TVShow))
            {
                return false;
            }

            var tvShowFromDb = dbContext.TVShows.SingleOrDefault(t => t.Name == input.TVShow);

            var seasonForDb = new Season
            {
                TVShow = tvShowFromDb,
                SeasonNumber = tvShowFromDb.Seasons.Count() + 1,
                ReleaseDate = input.ReleaseDate,
                Episodes = input.Episodes,
                LengthPerEpisode = input.LengthPerEpisode,
            };
            dbContext.Seasons.Add(seasonForDb);
            dbContext.SaveChanges();

            return true;
        }

        public List<SeasonsAndTVShowNameViewModel> GetAllSeasonsAndTVShowNames()
        {
            var tvShowsAndSeasonsViewModel = dbContext.Seasons.Select(season => new SeasonsAndTVShowNameViewModel
            {
                SeasonId = season.Id,
                SeasonNumber = season.SeasonNumber,
                TVShowName = season.TVShow.Name,
            }).ToList();

            return tvShowsAndSeasonsViewModel;
        }

        public bool AddRoleToTVShowSeason(AddRoleInputModel input)
        {
            if (!dbContext.Seasons.Any(season => season.Id == input.SeasonId))
            {
                return false;
            }
            if (!dbContext.Artists.Any(artist => artist.FullName == input.Artist))
            {
                return false;
            }

            var seasonFromDb = dbContext.Seasons.SingleOrDefault(s => s.Id == input.SeasonId);
            var artistFromDb = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Artist);

            if (dbContext.SeasonRoles.Any(sr => sr.ArtistId == artistFromDb.Id && sr.SeasonId == seasonFromDb.Id))
            {
                return false;
            }

            var seasonRoleForDb = new SeasonRole
            {
                Season = seasonFromDb,
                Artist = artistFromDb,
                CharacterPlayed = input.CharacterPlayed,
            };
            dbContext.SeasonRoles.Add(seasonRoleForDb);
            dbContext.SaveChanges();

            return true;
        }        
    }
}
