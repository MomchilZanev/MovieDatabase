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

        public IActionResult All()
        {
            var allTVShows = this.tvShowService.GetAllTVShows()
                .Select(t => new TVShowAllViewModel
                {
                    Name = t.Name,
                    Description = t.Description,
                    CoverImageLink = t.CoverImageLink,
                })
                .ToList();

            return View(allTVShows);
        }
    }
}