using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Announcement
{
    public class CreateAnnouncementInputModel
    {
        [Required]
        [StringLength(ValidationConstants.announcementTitleMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.announcementTitleMinimumLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ValidationConstants.announcementCreatorMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.announcementCreatorMinimumLength)]
        public string Creator { get; set; }

        [Required]
        [StringLength(ValidationConstants.announcementContentMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.announcementContentMinimumLength)]
        public string Content { get; set; }

        [Display(Name = "Image Link")]
        public string ImageLink { get; set; }

        [Required]
        [Display(Name = "Official Article Link")]
        public string OfficialArticleLink { get; set; }
    }
}
