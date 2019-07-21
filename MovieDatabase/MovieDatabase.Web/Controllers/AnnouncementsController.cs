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

        public IActionResult All(string orderBy)
        {
            var allAnnouncementsViewModel = announcementService.GetAllAnnouncements();

            if (!string.IsNullOrEmpty(orderBy))
            {
                allAnnouncementsViewModel = announcementService.OrderAnnouncements(allAnnouncementsViewModel, orderBy);
            }

            return View(allAnnouncementsViewModel);
        }
    }
}