using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class AnnouncementsController : AdministrationController
    {
        private const string redirectError = "/Home/Error";
        private const string redirectAnnuncementsAllAndOrder = "/Announcements/All/?orderBy=" + GlobalConstants.announcementsOrderByLatest;

        private readonly IAnnouncementService announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            this.announcementService = announcementService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]     
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

        public async Task<IActionResult> Delete(string id)
        {
            if (!await announcementService.DeleteAnnouncementAsync(id))
            {
                return Redirect(redirectError);
            }

            return Redirect(redirectAnnuncementsAllAndOrder);
        }
    }
}