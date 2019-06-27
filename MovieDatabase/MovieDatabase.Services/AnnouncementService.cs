using MovieDatabase.Data;
using MovieDatabase.Domain;
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

        public List<AnnouncementViewModel> GetAllAnnouncementsOrderedByDateAscending()
        {
            var allAnnouncements = dbContext.Announcements
                .Select(a => new AnnouncementViewModel
                {
                    Title = a.Title,
                    Content = a.Content,
                    ImageLink = a.ImageLink,
                    Date = a.Date
                })
                .OrderBy(a => a.Date)
                .ToList();

            return allAnnouncements;
        }
    }
}
