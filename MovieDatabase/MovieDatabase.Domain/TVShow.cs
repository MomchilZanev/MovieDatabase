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

        public Genre Genre { get; set; }

        public double OverallRating => this.Seasons.Average(season => season.Rating);

        [Required]
        public string CreatorId { get; set; }
        public Artist Creator { get; set; }

        public ICollection<Season> Seasons { get; set; }        
    }
}
