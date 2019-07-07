using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITVShowService tvShowService;
        private readonly IGenreService genreService;
        private readonly IArtistService artistService;

        public TVShowsController(ITVShowService tvShowService, IGenreService genreService, IArtistService artistService)
        {
            this.tvShowService = tvShowService;
            this.genreService = genreService;
            this.artistService = artistService;
        }

        public IActionResult Details(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var tvShowDetailsViewModel = tvShowService.GetTVShowAndDetailsById(id, userId);

            return View(tvShowDetailsViewModel);
        }

        public IActionResult All(string orderBy)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var allTVShowsViewModel = tvShowService.GetAllTVShowsAndOrder(orderBy, userId);

            return View(allTVShowsViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Genres = genreService.GetAllGenres();
            ViewBag.Creators = artistService.GetAllArtistsAndOrder("").Select(a => a.FullName).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateTVShowInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = genreService.GetAllGenres();
                ViewBag.Creators = artistService.GetAllArtistsAndOrder("").Select(a => a.FullName).ToList();

                return View(input);
            }

            tvShowService.CreateTVShow(input);

            return Redirect("/Home/Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddSeason()
        {
            ViewBag.TVShows = tvShowService.GetAllTVShowsAndOrder("", "").Select(a => a.Name).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSeason(AddSeasonInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TVShows = tvShowService.GetAllTVShowsAndOrder("", "").Select(a => a.Name).ToList();

                return View(input);
            }

            tvShowService.AddSeasonToTVShow(input);

            return Redirect("/Home/Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            ViewBag.Seasons = tvShowService.GetAllSeasonsAndTVShowNames();
            ViewBag.Artists = artistService.GetAllArtistsAndOrder("").Select(a => a.FullName).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole(AddRoleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Seasons = tvShowService.GetAllSeasonsAndTVShowNames();
                ViewBag.Artists = artistService.GetAllArtistsAndOrder("").Select(a => a.FullName).ToList();

                return View(input);
            }

            tvShowService.AddRoleToTVShowSeason(input);

            return Redirect("/Home/Index");
        }
    }
}