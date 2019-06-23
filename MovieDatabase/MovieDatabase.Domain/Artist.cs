using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Artist
    {
        public Artist()
        {
            this.MoviesDirected = new HashSet<Movie>();
            this.MovieRoles = new HashSet<MovieRole>();
            this.TVShowsCreated = new HashSet<TVShow>();
            this.SeasonRoles = new HashSet<SeasonRole>();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public string Biography { get; set; }

        public string PhotoLink { get; set; }

        public virtual ICollection<Movie> MoviesDirected { get; set; }

        public virtual ICollection<MovieRole> MovieRoles { get; set; }

        public virtual ICollection<TVShow> TVShowsCreated { get; set; }

        public virtual ICollection<SeasonRole> SeasonRoles { get; set; }

    }
}
