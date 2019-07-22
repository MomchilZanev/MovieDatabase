using AutoMapper;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class TVShowService : ITVShowService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IReviewService reviewService;
        private readonly IWatchlistService watchlistService;
        private readonly IMapper mapper;

        public TVShowService(MovieDatabaseDbContext dbContext, IReviewService reviewService, IWatchlistService watchlistService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.reviewService = reviewService;
            this.watchlistService = watchlistService;
            this.mapper = mapper;
        }

        public List<TVShowNameViewModel> GetAllTVShowNames()
        {
            var allTVShowsFromDb = dbContext.TVShows.ToList();

            var allTVShowNames = mapper.Map<List<TVShow>, List<TVShowNameViewModel>>(allTVShowsFromDb);

            return allTVShowNames;
        }

        public List<SeasonsAndTVShowNameViewModel> GetAllSeasonIdsSeasonNumbersAndTVShowNames()
        {
            var tvShowsAndSeasonsFromDb = dbContext.Seasons.ToList();

            var tvShowsAndSeasonsViewModel = mapper.Map<List<Season>, List<SeasonsAndTVShowNameViewModel>>(tvShowsAndSeasonsFromDb);

            return tvShowsAndSeasonsViewModel;
        }

        public List<TVShowAllViewModel> GetAllTVShows(string userId = null)
        {
            var allTVShowsFromDb = dbContext.TVShows.ToList();

            var tvShowAllViewModel = mapper.Map<List<TVShow>, List<TVShowAllViewModel>>(allTVShowsFromDb);
            foreach (var tvShow in tvShowAllViewModel)
            {
                tvShow.Watchlisted = watchlistService.TVShowIsInUserWatchlist(userId, tvShow.Id);
            }

            return tvShowAllViewModel;
        }

        public List<TVShowAllViewModel> FilterTVShowsByGenre(List<TVShowAllViewModel> tvShowsAllViewModel, string genreFilter)
        {
            if (dbContext.Genres.Any(genre => genre.Name == genreFilter))
            {
                tvShowsAllViewModel = tvShowsAllViewModel.Where(tvShow => tvShow.Genre == genreFilter).ToList();
            }

            return tvShowsAllViewModel.ToList();
        }

        public List<TVShowAllViewModel> OrderTVShows(List<TVShowAllViewModel> tvShowsAllViewModel, string orderBy)
        {
            switch (orderBy)
            {
                case GlobalConstants.moviesTvShowsOrderByRelease:
                    return tvShowsAllViewModel.OrderByDescending(tvShow => tvShow.FirstAired).ToList();
                case GlobalConstants.moviesTvShowsOrderByPopularity:
                    return tvShowsAllViewModel.OrderByDescending(tvShow => tvShow.TotalReviews).ToList();
                case GlobalConstants.moviesTvShowsOrderByRating:
                    return tvShowsAllViewModel.OrderByDescending(tvShow => tvShow.Rating).ToList();
                case GlobalConstants.moviesTvShowsShowComingSoon:
                    return tvShowsAllViewModel.Where(tvShow => tvShow.FirstAired > DateTime.UtcNow).OrderByDescending(tvShow => tvShow.FirstAired).ToList();
                default:
                    return tvShowsAllViewModel.ToList();
            }
        }        

        public async Task<TVShowDetailsViewModel> GetTVShowAndDetailsByIdAsync(string tvShowId)
        {
            var tvShowFromDb = await dbContext.TVShows.FindAsync(tvShowId);

            var tvShowDetailsViewModel = mapper.Map<TVShow, TVShowDetailsViewModel>(tvShowFromDb);

            if (tvShowFromDb.Seasons.Any())
            {
                foreach (var season in tvShowFromDb.Seasons.OrderBy(s => s.SeasonNumber))
                {
                    tvShowDetailsViewModel.Seasons.Add(season.Id, season.SeasonNumber);
                }
            }            

            return tvShowDetailsViewModel;
        }

        public async Task<SeasonDetailsViewModel> GetSeasonAndDetailsByIdAsync(string seasonId, string userId = null)
        {
            var seasonFromDb = await dbContext.Seasons.FindAsync(seasonId);

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

            var seasonDetailsViewModel = mapper.Map<Season, SeasonDetailsViewModel>(seasonFromDb);
            seasonDetailsViewModel.RandomReview = randomReview;
            seasonDetailsViewModel.Cast = mapper.Map<List<SeasonRole>, List<SeasonCastViewModel>>(seasonFromDb.Cast.ToList());
            seasonDetailsViewModel.IsReviewedByCurrentUser = reviewService.ReviewExists(userId, seasonFromDb.Id);

            return seasonDetailsViewModel;
        }

        public async Task<bool> CreateTVShowAsync(CreateTVShowInputModel input)
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

            var tvShowForDb = mapper.Map<CreateTVShowInputModel, TVShow>(input);
            tvShowForDb.Genre = genreFromDb;
            tvShowForDb.Creator = creatorFromDb;

            await dbContext.TVShows.AddAsync(tvShowForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddSeasonToTVShowAsync(AddSeasonInputModel input)
        {
            if (!dbContext.TVShows.Any(tvShow => tvShow.Name == input.TVShow))
            {
                return false;
            }

            var tvShowFromDb = dbContext.TVShows.SingleOrDefault(t => t.Name == input.TVShow);

            var seasonForDb = mapper.Map<AddSeasonInputModel, Season>(input);
            seasonForDb.TVShow = tvShowFromDb;
            seasonForDb.SeasonNumber = tvShowFromDb.Seasons.Count() + 1;

            await dbContext.Seasons.AddAsync(seasonForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> AddRoleToTVShowSeasonAsync(AddSeasonRoleInputModel input)
        {
            if (!dbContext.Seasons.Any(season => season.Id == input.SeasonId))
            {
                return false;
            }
            if (!dbContext.Artists.Any(artist => artist.FullName == input.Artist))
            {
                return false;
            }

            var seasonFromDb = dbContext.Seasons.SingleOrDefault(season => season.Id == input.SeasonId);
            var artistFromDb = dbContext.Artists.SingleOrDefault(artist => artist.FullName == input.Artist);

            if (dbContext.SeasonRoles.Any(sr => sr.ArtistId == artistFromDb.Id && sr.SeasonId == seasonFromDb.Id))
            {
                return false;
            }

            var seasonRoleForDb = mapper.Map<AddSeasonRoleInputModel, SeasonRole>(input);
            seasonRoleForDb.Season = seasonFromDb;
            seasonRoleForDb.Artist = artistFromDb;

            await dbContext.SeasonRoles.AddAsync(seasonRoleForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTVShowAsync(UpdateTVShowInputModel input)
        {
            if (!dbContext.TVShows.Any(tvShow => tvShow.Id == input.Id))
            {
                return false;
            }
            if (!dbContext.Genres.Any(genre => genre.Name == input.Genre))
            {
                return false;
            }
            if (!dbContext.Artists.Any(artist => artist.FullName == input.Creator))
            {
                return false;
            }

            var genreFromDb = dbContext.Genres.SingleOrDefault(g => g.Name == input.Genre);
            var creatorFromDb = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Creator);

            var tvShowFromDb = await dbContext.TVShows.FindAsync(input.Id);

            tvShowFromDb.Name = input.Name;
            tvShowFromDb.Genre = genreFromDb;
            tvShowFromDb.Creator = creatorFromDb;
            tvShowFromDb.Description = input.Description;
            tvShowFromDb.CoverImageLink = input.CoverImageLink;
            tvShowFromDb.TrailerLink = input.TrailerLink;

            dbContext.Update(tvShowFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateSeasonAsync(UpdateSeasonInputModel input)
        {
            if (!dbContext.Seasons.Any(season => season.Id == input.Id))
            {
                return false;
            }
            if (!dbContext.TVShows.Any(tvShow => tvShow.Name == input.TVShow))
            {
                return false;
            }

            var tvShowFromDb = dbContext.TVShows.SingleOrDefault(tvShow => tvShow.Name == input.TVShow);
            
            var seasonFromDb = dbContext.Seasons.SingleOrDefault(season => season.Id == input.Id);
            if (tvShowFromDb != seasonFromDb.TVShow)
            {
                seasonFromDb.SeasonNumber = tvShowFromDb.Seasons.Count() + 1;
            }
            seasonFromDb.TVShow = tvShowFromDb;
            seasonFromDb.ReleaseDate = input.ReleaseDate;
            seasonFromDb.Episodes = input.Episodes;
            seasonFromDb.LengthPerEpisode = input.LengthPerEpisode;

            dbContext.Update(seasonFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}