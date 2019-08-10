using MovieDatabase.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Movie
{
    public class CreateMovieInputModel
    {

        [Required]
        [StringLength(ValidationConstants.movieNameMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.movieNameMinimumLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [Range(ValidationConstants.movieMinimumLengthInMinutes, ValidationConstants.movieMaximumLengthInMinutes)]
        [Display(Name = "Length (in minutes)")]
        public int Length { get; set; }

        [Required]
        [StringLength(ValidationConstants.movieDescriptionMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.movieDescriptionMinimumLength)]
        public string Description { get; set; }

        [Display(Name = "Cover Image Link")]
        public string CoverImageLink { get; set; }

        [Display(Name = "Trailer Link")]
        public string TrailerLink { get; set; }

        [Required]
        public string Director { get; set; }
    }
}
