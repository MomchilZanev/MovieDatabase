using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public AnnouncementService(MovieDatabaseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<AnnouncementViewModel>> GetAllAnnouncementsAsync()
        {
            var announcementsFromDb = await dbContext.Announcements.ToListAsync();

            var announcementsAllViewModel = mapper.Map<List<Announcement>, List<AnnouncementViewModel>>(announcementsFromDb);

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
            if (await dbContext.Announcements.AnyAsync(announcement => announcement.Title == input.Title && announcement.Content == input.Content))
            {
                return false;
            }

            var announcementFromDb = mapper.Map<CreateAnnouncementInputModel, Announcement>(input);

            await dbContext.Announcements.AddAsync(announcementFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAnnouncementAsync(string announcementId)
        {
            if (!await dbContext.Announcements.AnyAsync(announcement => announcement.Id == announcementId))
            {
                return false;
            }

            var announcementFromDb = await dbContext.Announcements.SingleOrDefaultAsync(announcement => announcement.Id == announcementId);

            dbContext.Announcements.Remove(announcementFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
