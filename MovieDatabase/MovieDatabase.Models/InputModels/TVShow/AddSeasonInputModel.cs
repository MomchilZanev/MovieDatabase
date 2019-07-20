using MovieDatabase.Common;
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
        [Range(ValidationConstants.seasonMinimumEpisodes, ValidationConstants.seasonMaximumEpisodes)]
        [Display(Name = "Number of Episodes")]
        public int Episodes { get; set; }

        [Required]
        [Range(ValidationConstants.seasonMinimumLengthPerEpisode, ValidationConstants.seasonMaximumLengthPerEpisode)]
        [Display(Name = "Length per Episode (in minutes)")]
        public int LengthPerEpisode { get; set; }
    }
}
