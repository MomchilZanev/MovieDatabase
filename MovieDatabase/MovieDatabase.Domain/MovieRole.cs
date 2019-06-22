using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class MovieRole
    {
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }//[ForeignKey("MovieId")] Just a reminder

        [Required]
        public string ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        public string CharacterPlayed { get; set; }
    }
}
