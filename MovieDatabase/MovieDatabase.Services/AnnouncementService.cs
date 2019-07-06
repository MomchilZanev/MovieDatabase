using Microsoft.AspNetCore.Http;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
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
                    Content = a.Content.Substring(0, 500) + "....",
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

        public bool CreateAnnouncement(CreateAnnouncementInputModel input)
        {
            if (dbContext.Announcements.Any(a => a.Title == input.Title && a.Content == input.Content))
            {
                return false;
            }            

            var announcement = new Announcement
            {
                Creator = input.Creator,
                Title = input.Title,
                Content = input.Content,
                OfficialArticleLink = input.OfficialArticleLink,
                ImageLink = (input.ImageLink == "" || input.ImageLink == null) ? "/images/no_image.png" : input.ImageLink,
                Date = DateTime.UtcNow,
            };
            dbContext.Announcements.Add(announcement);
            dbContext.SaveChanges();
            
            return true;
        }
    }
}
