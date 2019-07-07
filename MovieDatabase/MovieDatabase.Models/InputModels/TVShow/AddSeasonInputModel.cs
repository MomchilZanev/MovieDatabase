using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.TVShow
{
    public class AddSeasonInputModel
    {
        [Required]
        public string TVShow { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(3, 44)]
        [Display(Name = "Number of Episodes")]
        public int Episodes { get; set; }

        [Required]
        [Range(20, 120)]
        [Display(Name = "Length per Episode (in minutes)")]
        public int LengthPerEpisode { get; set; }
    }
}
