using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Announcement
{
    public class CreateAnnouncementInputModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Creator { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; }

        [Display(Name = "Image Link")]
        public string ImageLink { get; set; }
        
        [Required]
        [Display(Name = "Official Article Link")]
        public string OfficialArticleLink { get; set; }
    }
}
