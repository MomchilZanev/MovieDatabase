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
        [StringLength(10000, MinimumLength = 10)]
        public string Content { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
