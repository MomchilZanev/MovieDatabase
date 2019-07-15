using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using MovieDatabase.Services.Contracts;
using System;
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

        public List<AnnouncementViewModel> GetAllAnnouncements()
        {
            var announcementsAllViewModel = dbContext.Announcements
                .Select(announcement => new AnnouncementViewModel
                {
                    Title = announcement.Title,
                    Content = announcement.Content + "....",
                    ImageLink = announcement.ImageLink,
                    Date = announcement.Date,
                    Creator = announcement.Creator,
                    OfficialArticleLink = announcement.OfficialArticleLink,
                })
                .OrderBy(a => a.Date)
                .ToList();

            return announcementsAllViewModel;
        }

        public List<AnnouncementViewModel> OrderAnnouncements(List<AnnouncementViewModel> announcements, string orderBy)
        {
            switch (orderBy)
            {
                case "latest":
                    return announcements.OrderByDescending(announcment => announcment.Date).ToList();
                case "oldest":
                    return announcements.OrderBy(announcement => announcement.Date).ToList();
                default:
                    return announcements.ToList();
            }
        }

        public bool CreateAnnouncement(CreateAnnouncementInputModel input)
        {
            if (dbContext.Announcements.Any(announcement => announcement.Title == input.Title && announcement.Content == input.Content))
            {
                return false;
            }            

            var announcementForDb = new Announcement
            {
                Creator = input.Creator,
                Title = input.Title,
                Content = input.Content,
                OfficialArticleLink = input.OfficialArticleLink,
                ImageLink = string.IsNullOrEmpty(input.ImageLink) ? "/images/no_image.png" : input.ImageLink,
                Date = DateTime.UtcNow,
            };
            dbContext.Announcements.Add(announcementForDb);
            dbContext.SaveChanges();
            
            return true;
        }
    }
}
