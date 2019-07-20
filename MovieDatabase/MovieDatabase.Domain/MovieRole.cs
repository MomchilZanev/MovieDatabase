using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class MovieRole
    {
        [Required]
        public string MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        [Required]
        [StringLength(ValidationConstants.roleCharacterPlayedMaximumLength, MinimumLength = ValidationConstants.roleCharacterPlayedMinimumLength)]
        public string CharacterPlayed { get; set; }
    }
}
