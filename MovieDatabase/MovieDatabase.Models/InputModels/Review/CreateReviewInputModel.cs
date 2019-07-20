using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Review
{
    public class CreateReviewInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.reviewContentMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.reviewContentMinimumLength)]
        public string Content { get; set; }

        [Range(ValidationConstants.reviewMinimumRating, ValidationConstants.reviewMaximumRating, ErrorMessage = "The {0} must be between {1} and {2}")]
        public int Rating { get; set; }
    }
}
