using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Genre
{
    public class CreateGenreInputModel
    {
        [Required]
        [Display(Name = "Genre Name")]
        [StringLength(ValidationConstants.genreNameMaximumLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ValidationConstants.genreNameMinimumLength)]
        public string Name { get; set; }
    }
}
