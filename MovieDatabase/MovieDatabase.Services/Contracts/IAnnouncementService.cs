using MovieDatabase.Domain;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IAnnouncementService
    {
        List<Announcement> GetAllAnnouncements();
    }
}
