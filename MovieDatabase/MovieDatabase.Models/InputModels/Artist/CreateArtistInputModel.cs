using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Artist
{
    public class CreateArtistInputModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 25)]
        public string Biography { get; set; }

        [Display(Name = "Photo Link")]
        public string PhotoLink { get; set; }
    }
}
