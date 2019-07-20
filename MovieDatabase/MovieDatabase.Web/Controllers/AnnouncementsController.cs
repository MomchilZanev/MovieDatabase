using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class AnnouncementsController : Controller
    {
        private const string redirectAnnuncementsAllAndOrder = "/Announcements/All/?orderBy=" + GlobalConstants.announcementsOrderByLatest;

        private readonly IAnnouncementService announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            this.announcementService = announcementService;
        }

        public IActionResult All(string orderBy)
        {
            var allAnnouncementsViewModel = announcementService.GetAllAnnouncements();

            if (!string.IsNullOrEmpty(orderBy))
            {
                allAnnouncementsViewModel = announcementService.OrderAnnouncements(allAnnouncementsViewModel, orderBy);
            }

            return View(allAnnouncementsViewModel);
        }

        [Authorize(Roles = GlobalConstants.adminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.adminRoleName)]        
        public async Task<IActionResult> Create(CreateAnnouncementInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await announcementService.CreateAnnouncementAsync(input))
            {
                return View(input);
            }            

            return Redirect(redirectAnnuncementsAllAndOrder);
        }
    }
}