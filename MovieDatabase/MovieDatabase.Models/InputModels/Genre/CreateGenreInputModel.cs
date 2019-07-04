using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.Genre
{
    public class CreateGenreInputModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}
