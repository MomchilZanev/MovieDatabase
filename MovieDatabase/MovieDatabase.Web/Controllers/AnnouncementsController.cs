using Microsoft.AspNetCore.Mvc;
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

        public IActionResult All()
        {
            var allAnnouncements = announcementService.GetAllAnnouncementsOrderedByDateAscending();

            return View(allAnnouncements);
        }
    }
}