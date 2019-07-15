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
            this.Cast = new HashSet<SeasonRole>();
            this.Reviews = new HashSet<SeasonReview>();
        }

        [Key]
        public string Id { get; set; }

        public int SeasonNumber { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Episodes { get; set; }

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
