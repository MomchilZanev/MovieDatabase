using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Movie
{
    public class CreateMovieInputModel
    {

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [Range(60, 300)]
        [Display(Name = "Length (in minutes)")]
        public int Length { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 25)]
        public string Description { get; set; }     

        [Display(Name = "Cover Image Link")]
        public string CoverImageLink { get; set; }
        
        [Required]
        public string Director { get; set; }
    }
}
