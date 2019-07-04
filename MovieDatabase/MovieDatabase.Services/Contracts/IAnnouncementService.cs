using MovieDatabase.Models.InputModels.Announcement;
using MovieDatabase.Models.ViewModels.Announcement;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IAnnouncementService
    {
        List<AnnouncementViewModel> GetAllAnnouncementsAndOrder(string orderBy);

        bool CreateAnnouncement(CreateAnnouncementInputModel input);
    }
}
