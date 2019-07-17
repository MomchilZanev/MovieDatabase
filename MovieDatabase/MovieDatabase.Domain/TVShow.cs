using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieDatabase.Domain
{
    public class TVShow
    {
        public TVShow()
        {
            Seasons = new HashSet<Season>();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }        

        [Required]
        [StringLength(1000, MinimumLength = 25)]
        public string Description { get; set; }        

        public virtual Genre Genre { get; set; }

        [Required]
        public string CoverImageLink { get; set; }

        [Required]
        public string TrailerLink { get; set; }

        public DateTime FirstAired => Seasons.Any() ? Seasons.OrderBy(s => s.ReleaseDate).First().ReleaseDate : DateTime.MaxValue;

        public double OverallRating => Seasons.Any() ? Seasons.Average(season => season.Rating) : 0;

        public int TotalReviews => Seasons.Any() ? Seasons.Sum(season => season.TotalReviews) : 0;

        [Required]
        public string CreatorId { get; set; }
        public virtual Artist Creator { get; set; }

        public virtual ICollection<Season> Seasons { get; set; }        
    }
}
