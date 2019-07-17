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
            Cast = new HashSet<MovieRole>();
            Reviews = new HashSet<MovieReview>();
        }

        [Required]
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 25)]
        public string Description { get; set; }

        [Range(60, 300)]
        public int Length { get; set; }

        public virtual Genre Genre { get; set; }

        [Required]
        public string CoverImageLink { get; set; }

        [Required]
        public string TrailerLink { get; set; }

        public double Rating => Reviews.Any() ? Reviews.Average(review => review.Rating) : 0;

        public int TotalReviews => Reviews.Count();

        [Required]
        public string DirectorId { get; set; }
        public virtual Artist Director { get; set; }

        public virtual ICollection<MovieRole> Cast { get; set; }

        public virtual ICollection<MovieReview> Reviews { get; set; }
    }
}
