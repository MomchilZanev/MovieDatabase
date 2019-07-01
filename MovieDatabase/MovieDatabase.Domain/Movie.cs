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

        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Description { get; set; }

        public int Length { get; set; }

        public virtual Genre Genre { get; set; }

        public string CoverImageLink { get; set; }

        public double Rating => this.Reviews.Any() ? this.Reviews.Average(review => review.Rating) : 0;

        [Required]
        public string DirectorId { get; set; }
        public virtual Artist Director { get; set; }

        public virtual ICollection<MovieRole> Cast { get; set; }

        public virtual ICollection<MovieReview> Reviews { get; set; }
    }
}
