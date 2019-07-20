using MovieDatabase.Common;
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
        [StringLength(ValidationConstants.roleCharacterPlayedMaximumLength, MinimumLength = ValidationConstants.roleCharacterPlayedMinimumLength)]
        public string CharacterPlayed { get; set; }
    }
}
