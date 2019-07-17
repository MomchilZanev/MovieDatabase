using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Services.Contracts;
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

        public IActionResult All(string orderBy, string genreFilter)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var tvShowsAllViewModel = tvShowService.GetAllTVShows(userId);

            if (!string.IsNullOrEmpty(genreFilter))
            {
                tvShowsAllViewModel = tvShowService.FilterTVShowsByGenre(tvShowsAllViewModel, genreFilter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                tvShowsAllViewModel = tvShowService.OrderTVShows(tvShowsAllViewModel, orderBy);
            }

            return View(tvShowsAllViewModel);
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

        public IActionResult SeasonDetails(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var seasonDetailsViewModel = tvShowService.GetSeasonAndDetailsById(id, userId);

            return View(seasonDetailsViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateTVShowInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!tvShowService.CreateTVShow(input))
            {
                return View(input);
            }

            return Redirect("/TVShows/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddSeason()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSeason(AddSeasonInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!tvShowService.AddSeasonToTVShow(input))
            {
                return View(input);
            }

            return Redirect("/TVShows/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole(AddRoleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!tvShowService.AddRoleToTVShowSeason(input))
            {
                return View(input);
            }

            return Redirect("/TVShows/All?orderBy=release");
        }
    }
}