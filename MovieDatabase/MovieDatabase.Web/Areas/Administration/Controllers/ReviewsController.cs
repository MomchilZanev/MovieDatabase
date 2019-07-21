﻿using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class ReviewsController : AdministrationController
    {
        private const string redirectError = "/Home/Error";
        private const string redirectMovieDetails = "/Movies/Details/";
        private const string redirectSeasonDetails = "/TVShows/SeasonDetails/";

        private readonly IReviewService reviewService;
        private readonly IUserService userService;

        public ReviewsController(IReviewService reviewService, IUserService userService)
        {
            this.reviewService = reviewService;
            this.userService = userService;
        }

        public async Task<IActionResult> Delete(string userName, string itemId)
        {
            bool idIsValidMovieOrSeasonId = reviewService.IsValidMovieOrSeasonId(itemId);
            if (!idIsValidMovieOrSeasonId)
            {
                return Redirect(redirectError);
            }

            var userId = userService.GetUserIdFromUserName(userName);

            var itemType = reviewService.IsIdMovieOrSeasonId(itemId);
            if (itemType == GlobalConstants.Movie)
            {
                if (!await reviewService.DeleteMovieReviewAsync(userId, itemId))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectMovieDetails + itemId);
            }
            else if (itemType == GlobalConstants.Season)
            {
                if (!await reviewService.DeleteSeasonReviewAsync(userId, itemId))
                {
                    return Redirect(redirectError);
                }

                return Redirect(redirectSeasonDetails + itemId);
            }
            else
            { return Redirect(redirectError); }
        }
    }
}