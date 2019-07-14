using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.TVShow
{
    public class AddRoleInputModel
    {
        [Required]
        [Display(Name = "Season")]
        public string SeasonId { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        [Display(Name = "Character Played")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string CharacterPlayed { get; set; }
    }
}
