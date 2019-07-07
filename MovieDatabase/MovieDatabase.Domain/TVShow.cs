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
            this.Seasons = new HashSet<Season>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }        

        [Required]
        public string Description { get; set; }        

        public virtual Genre Genre { get; set; }

        public string CoverImageLink { get; set; }

        public DateTime FirstAired => Seasons.Any() ? Seasons.First().ReleaseDate : DateTime.MaxValue;

        public double OverallRating => Seasons.Any() ? Seasons.Average(season => season.Rating) : 0;

        [Required]
        public string CreatorId { get; set; }
        public virtual Artist Creator { get; set; }

        public virtual ICollection<Season> Seasons { get; set; }        
    }
}
