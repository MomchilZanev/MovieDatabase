using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IAnnouncementService
    {
        Task<List<AnnouncementViewModel>> GetAllAnnouncementsAsync();

        List<AnnouncementViewModel> OrderAnnouncements(List<AnnouncementViewModel> announcements, string orderBy);

        Task<bool> CreateAnnouncementAsync(CreateAnnouncementInputModel input);

        Task<bool> DeleteAnnouncementAsync(string announcementId);
    }
}
