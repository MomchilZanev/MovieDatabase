using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementService announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            this.announcementService = announcementService;
        }

        public async Task<IActionResult> All(string orderBy)
        {
            var allAnnouncementsViewModel = await announcementService.GetAllAnnouncementsAsync();

            allAnnouncementsViewModel = announcementService.OrderAnnouncements(allAnnouncementsViewModel, orderBy);

            return View(allAnnouncementsViewModel);
        }
    }
}