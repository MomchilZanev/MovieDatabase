using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Announcement
{
    public class CreateAnnouncementInputModel
    {
        [Required]
        [StringLength(45, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Creator { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; }

        public string ImageLink { get; set; }

        [Required]
        public string OfficialArticleLink { get; set; }
    }
}
