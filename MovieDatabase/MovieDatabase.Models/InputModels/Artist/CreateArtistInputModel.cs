using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Artist
{
    public class CreateArtistInputModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 25)]
        public string Biography { get; set; }

        public string PhotoLink { get; set; }
    }
}
