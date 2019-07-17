﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Artist
    {
        public Artist()
        {
            MoviesDirected = new HashSet<Movie>();
            MovieRoles = new HashSet<MovieRole>();
            TVShowsCreated = new HashSet<TVShow>();
            SeasonRoles = new HashSet<SeasonRole>();
        }

        [Required]
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 25)]
        public string Biography { get; set; }

        [Required]
        public string PhotoLink { get; set; }

        public virtual ICollection<Movie> MoviesDirected { get; set; }

        public virtual ICollection<MovieRole> MovieRoles { get; set; }

        public virtual ICollection<TVShow> TVShowsCreated { get; set; }

        public virtual ICollection<SeasonRole> SeasonRoles { get; set; }

    }
}
