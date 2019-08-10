using MovieDatabase.Common;
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
        [StringLength(ValidationConstants.tvShowNameMaximumLength, MinimumLength = ValidationConstants.tvShowNameMinimumLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ValidationConstants.tvShowDescriptionMaximumLength, MinimumLength = ValidationConstants.tvShowDescriptionMinimumLength)]
        public string Description { get; set; }

        [Required]
        public string GenreId { get; set; }
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
