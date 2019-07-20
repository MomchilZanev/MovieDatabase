using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.TVShow
{
    public class CreateTVShowInputModel
    {

        [Required]
        [StringLength(ValidationConstants.tvShowNameMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.tvShowNameMinimumLength)]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [StringLength(ValidationConstants.tvShowDescriptionMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.tvShowDescriptionMinimumLength)]
        public string Description { get; set; }     

        [Display(Name = "Cover Image Link")]
        public string CoverImageLink { get; set; }

        [Display(Name = "Trailer Link")]
        public string TrailerLink { get; set; }

        [Required]
        public string Creator { get; set; }
    }
}
