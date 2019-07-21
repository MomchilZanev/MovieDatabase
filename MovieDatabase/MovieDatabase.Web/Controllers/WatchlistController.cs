﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class WatchlistController : Controller
    {
        private const string redirectError = "/Home/Error";

        private readonly IWatchlistService watchlistService;
        private readonly IUserService userService;

        public WatchlistController(IWatchlistService watchlistService, IUserService userService)
        {
            this.watchlistService = watchlistService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = userService.GetUserIdFromUserName(User.Identity.Name);

            var watchlistAllViewModel = watchlistService.GetItemsInUserWatchlist(userId);

            return View(watchlistAllViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Add(string id, string returnAction, string returnQuery)
        {
            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (!idIsValidMovieOrTVShowId)
            {
                return Redirect(redirectError);
            }

            var userId = userService.GetUserIdFromUserName(User.Identity.Name);

            var itemType = watchlistService.IsIdMovieOrTVShowId(id);
            if (itemType == GlobalConstants.Movie)
            {
                if (watchlistService.MovieIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.AddMovieToUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == GlobalConstants.TV_Show)
            {
                if (watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.AddTVShowToUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect(redirectError); }
        }

        [Authorize]
        public async Task<IActionResult> Remove(string id, string returnAction, string returnQuery)
        {
            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (!idIsValidMovieOrTVShowId)
            {
                return Redirect(redirectError);
            }

            var userId = userService.GetUserIdFromUserName(User.Identity.Name);

            var itemType = watchlistService.IsIdMovieOrTVShowId(id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!watchlistService.MovieIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.RemoveMovieFromUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == GlobalConstants.TV_Show)
            {
                if (!watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.RemoveTVShowFromUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect(redirectError); }
        }
    }
}