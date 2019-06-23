using MovieDatabase.Data;
using MovieDatabase.Domain;
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

        public List<Announcement> GetAllAnnouncements()
        {
            return this.dbContext.Announcements.ToList();
        }
    }
}
