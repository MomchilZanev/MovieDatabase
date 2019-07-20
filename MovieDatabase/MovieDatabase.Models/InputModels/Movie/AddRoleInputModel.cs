using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Movie
{
    public class AddRoleInputModel
    {
        [Required]
        public string Movie { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        [Display(Name = "Character Played")]
        [StringLength(ValidationConstants.roleCharacterPlayedMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.roleCharacterPlayedMinimumLength)]
        public string CharacterPlayed { get; set; }
    }
}
