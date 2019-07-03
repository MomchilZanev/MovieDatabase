using MovieDatabase.Data;
using MovieDatabase.Models.ViewModels.Announcement;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public AnnouncementService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<AnnouncementViewModel> GetAllAnnouncementsAndOrder(string orderBy)
        {
            var announcementAllViewModel = dbContext.Announcements
                .Select(a => new AnnouncementViewModel
                {
                    Title = a.Title,
                    Content = a.Content.Substring(0, 260) + "....",
                    ImageLink = a.ImageLink,
                    Date = a.Date
                })
                .OrderBy(a => a.Date)
                .ToList();

            if (orderBy == "latest")
            {
                announcementAllViewModel = announcementAllViewModel
                    .OrderByDescending(a => a.Date)
                    .ToList();
            }
            else if (orderBy == "oldest")
            {
                announcementAllViewModel = announcementAllViewModel
                    .OrderBy(a => a.Date)
                    .ToList();
            }

            return announcementAllViewModel;
        }

    }
}
