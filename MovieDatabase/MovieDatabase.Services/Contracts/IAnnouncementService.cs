using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IAnnouncementService
    {
        List<AnnouncementViewModel> GetAllAnnouncements();

        List<AnnouncementViewModel> OrderAnnouncements(List<AnnouncementViewModel> announcements, string orderBy);

        bool CreateAnnouncement(CreateAnnouncementInputModel input);
    }
}
