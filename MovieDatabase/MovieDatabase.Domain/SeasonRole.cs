using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class SeasonRole
    {
        [Required]
        public string SeasonId { get; set; }
        public virtual Season Season { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string CharacterPlayed { get; set; }
    }
}
