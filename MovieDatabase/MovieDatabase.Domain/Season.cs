using MovieDatabase.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieDatabase.Domain
{
    public class Season
    {
        public Season()
        {
            Cast = new HashSet<SeasonRole>();
            Reviews = new HashSet<SeasonReview>();
        }

        [Required]
        [Key]
        public string Id { get; set; }

        [Range(ValidationConstants.seasonMinimumSeasonNumber, ValidationConstants.seasonMaximumSeasonNumber)]
        public int SeasonNumber { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Range(ValidationConstants.seasonMinimumEpisodes, ValidationConstants.seasonMaximumEpisodes)]
        public int Episodes { get; set; }

        [Range(ValidationConstants.seasonMinimumLengthPerEpisode, ValidationConstants.seasonMaximumLengthPerEpisode)]
        public int LengthPerEpisode { get; set; }

        public double Rating => Reviews.Any() ? Reviews.Average(review => review.Rating) : 0;

        public int TotalReviews => Reviews.Count();

        [Required]
        public string TVShowId { get; set; }
        public virtual TVShow TVShow { get; set; }

        public virtual ICollection<SeasonRole> Cast { get; set; }

        public virtual ICollection<SeasonReview> Reviews { get; set; }
    }
}
