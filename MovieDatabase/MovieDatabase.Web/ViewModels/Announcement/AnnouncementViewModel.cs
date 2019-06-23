using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Web.ViewModels.Announcement
{
    public class AnnouncementViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageLink { get; set; }

        public DateTime Date { get; set; }
    }
}
