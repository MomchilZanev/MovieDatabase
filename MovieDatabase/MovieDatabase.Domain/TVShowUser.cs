using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class TVShowUser
    {
        [Required]
        public string TVShowId { get; set; }
        public virtual TVShow TVShow { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual MovieDatabaseUser User { get; set; }
    }
}
