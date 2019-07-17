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

        [Range(1, 2147483647)]
        public int SeasonNumber { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Range(3, 44)]
        public int Episodes { get; set; }

        [Range(20, 120)]
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
