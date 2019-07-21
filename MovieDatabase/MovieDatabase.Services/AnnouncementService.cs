using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    Id = announcement.Id,
                    Title = announcement.Title,
                    Content = announcement.Content + GlobalConstants.fourDots,
                    ImageLink = announcement.ImageLink,
                    Date = announcement.Date,
                    Creator = announcement.Creator,
                    OfficialArticleLink = announcement.OfficialArticleLink,
                })
                .ToList();

            return announcementsAllViewModel;
        }

        public List<AnnouncementViewModel> OrderAnnouncements(List<AnnouncementViewModel> announcements, string orderBy)
        {
            switch (orderBy)
            {
                case GlobalConstants.announcementsOrderByLatest:
                    return announcements.OrderByDescending(announcment => announcment.Date).ToList();
                case GlobalConstants.announcementsOrderByOldest:
                    return announcements.OrderBy(announcement => announcement.Date).ToList();
                default:
                    return announcements.ToList();
            }
        }

        public async Task<bool> CreateAnnouncementAsync(CreateAnnouncementInputModel input)
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
                ImageLink = string.IsNullOrEmpty(input.ImageLink) ? GlobalConstants.noImageLink : input.ImageLink,
                Date = DateTime.UtcNow,
            };
            await dbContext.Announcements.AddAsync(announcementForDb);
            await dbContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteAnnouncementAsync(string announcementId)
        {
            if (!dbContext.Announcements.Any(announcement => announcement.Id == announcementId))
            {
                return false;
            }

            var announcementFromDb = dbContext.Announcements.SingleOrDefault(announcement => announcement.Id == announcementId);

            dbContext.Announcements.Remove(announcementFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
