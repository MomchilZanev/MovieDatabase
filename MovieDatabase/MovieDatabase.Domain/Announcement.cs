using MovieDatabase.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Announcement
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.announcementTitleMaximumLength, MinimumLength = ValidationConstants.announcementTitleMinimumLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ValidationConstants.announcementCreatorMaximumLength, MinimumLength = ValidationConstants.announcementCreatorMinimumLength)]
        public string Creator { get; set; }

        [Required]
        [StringLength(ValidationConstants.announcementContentMaximumLength, MinimumLength = ValidationConstants.announcementContentMinimumLength)]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public string ImageLink { get; set; }

        [Required]
        public string OfficialArticleLink { get; set; }
    }
}
