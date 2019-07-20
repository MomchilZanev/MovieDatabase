using MovieDatabase.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class MovieReview
    {
        [Required]
        public string MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual MovieDatabaseUser User { get; set; }

        [Required]
        [StringLength(ValidationConstants.reviewContentMaximumLength, MinimumLength = ValidationConstants.reviewContentMinimumLength)]
        public string Content { get; set; }

        [Range(ValidationConstants.reviewMinimumRating, ValidationConstants.reviewMaximumRating)]
        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
