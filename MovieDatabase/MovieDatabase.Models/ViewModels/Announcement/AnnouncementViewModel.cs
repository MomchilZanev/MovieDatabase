using System;

namespace MovieDatabase.Models.ViewModels.Announcement
{
    public class AnnouncementViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageLink { get; set; }

        public DateTime Date { get; set; }

        public string Creator { get; set; }

        public string OfficialArticleLink { get; set; }
    }
}
