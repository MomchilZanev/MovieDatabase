using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Services.Contracts;

namespace MovieDatabase.Web.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementService announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            this.announcementService = announcementService;
        }

        public IActionResult All(string orderBy)
        {
            var allAnnouncements = announcementService.GetAllAnnouncementsAndOrder(orderBy);

            return View(allAnnouncements);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]        
        public IActionResult Create(CreateAnnouncementInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }            

            announcementService.CreateAnnouncement(input);

            return Redirect("/Announcements/All/?orderBy=latest");
        }
    }
}