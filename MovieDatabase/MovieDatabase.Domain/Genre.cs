using MovieDatabase.Common;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Genre
    {
        [Required]
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.genreNameMaximumLength, MinimumLength = ValidationConstants.genreNameMinimumLength)]
        public string Name { get; set; }
    }
}
