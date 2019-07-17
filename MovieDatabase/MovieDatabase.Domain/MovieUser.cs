using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class MovieUser
    {
        [Required]
        public string MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual MovieDatabaseUser User { get; set; }
    }
}
