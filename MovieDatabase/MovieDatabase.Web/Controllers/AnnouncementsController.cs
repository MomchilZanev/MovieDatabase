using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using MovieDatabase.Web.ViewModels.Announcement;
using System.Linq;

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
            var allAnnouncements = announcementService.GetAllAnnouncements()
                .Select(a => new AnnouncementViewModel
                {
                    Title = a.Title,
                    Content = a.Content,
                    ImageLink = a.ImageLink,
                    Date = a.Date
                })
                .OrderBy(a => a.Date)
                .ToList();

            return View(allAnnouncements);
        }
    }
}