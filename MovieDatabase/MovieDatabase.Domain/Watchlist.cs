using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Watchlist
    {
        public Watchlist()
        {
            this.MoviesToWatch = new HashSet<Movie>();
            this.TVShowsToWatch = new HashSet<TVShow>();
        }

        [Key]
        public string Id { get; set; }
        
        public ICollection<Movie> MoviesToWatch { get; set; }

        public ICollection<TVShow> TVShowsToWatch { get; set; }
    }
}
