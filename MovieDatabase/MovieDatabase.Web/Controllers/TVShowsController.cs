using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using MovieDatabase.Web.ViewModels.TVShow;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITVShowService tvShowService;

        public TVShowsController(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
        }

        public IActionResult All(string orderBy)
        {
            var allTVShows = this.tvShowService.GetAllTVShows()
                .Select(t => new TVShowAllViewModel
                {
                    Name = t.Name,
                    Description = t.Description,
                    CoverImageLink = t.CoverImageLink,
                    ReleaseDate = t.Seasons.First().ReleaseDate,
                    Rating = t.OverallRating,
                    TotalReviews = t.Seasons.Sum(s => s.Reviews.Count())
                })
                .ToList();

            //TODO: move this logic in Service Layer
            if (orderBy == "release")
            {
                allTVShows = allTVShows
                    .Where(t => t.ReleaseDate != null)
                    .OrderByDescending(t => t.ReleaseDate)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                allTVShows = allTVShows
                    .Where(t => t.ReleaseDate != null)
                    .OrderByDescending(t => t.TotalReviews)
                    .ToList();
            }
            else if (orderBy == "rating")
            {
                allTVShows = allTVShows
                    .Where(t => t.ReleaseDate != null)
                    .OrderByDescending(t => t.Rating)
                    .ToList();
            }
            else if (orderBy == "soon")
            {
                allTVShows = allTVShows
                    .Where(m => m.ReleaseDate != null && m.ReleaseDate > DateTime.UtcNow)
                    .OrderBy(m => m.ReleaseDate)
                    .ToList();
            }

            return View(allTVShows);
        }
    }
}