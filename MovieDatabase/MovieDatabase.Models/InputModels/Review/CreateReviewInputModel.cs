using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Review
{
    public class CreateReviewInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; }

        [Range(1, 10, ErrorMessage = "The {0} must be between {1} and {2}")]
        public int Rating { get; set; }
    }
}
