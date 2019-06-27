using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Announcement;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IAnnouncementService
    {
        List<AnnouncementViewModel> GetAllAnnouncementsOrderedByDateAscending();
    }
}
