using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieDatabase.Domain
{
    public class Movie
    {
        public Movie()
        {
            this.Cast = new HashSet<MovieRole>();
            this.Reviews = new HashSet<MovieReview>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Required]
        public string Description { get; set; }

        public int Length { get; set; }

        public Genre Genre { get; set; }

        public string CoverImageLink { get; set; }

        public double Rating => this.Reviews.Average(review => review.Rating);

        [Required]
        public string DirectorId { get; set; }
        public Artist Director { get; set; }

        public ICollection<MovieRole> Cast { get; set; }

        public ICollection<MovieReview> Reviews { get; set; }
    }
}
