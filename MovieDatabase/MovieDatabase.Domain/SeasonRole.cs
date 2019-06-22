using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class SeasonRole
    {
        [Required]
        public string SeasonId { get; set; }
        public Season Season { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Required]
        public string CharacterPlayed { get; set; }
    }
}
