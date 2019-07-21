using MovieDatabase.Common;
using MovieDatabase.Models.CustomValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Artist
{
    public class UpdateArtistInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(ValidationConstants.artistFullNameMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.artistFullNameMinimumLength)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [BirthDayBeforeToday(ErrorMessage = "Artist cannot be born after current date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(ValidationConstants.artistBiographyMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.artistBiographyMinimumLength)]
        public string Biography { get; set; }

        [Display(Name = "Photo Link")]
        public string PhotoLink { get; set; }
    }
}
