using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Genre
    {
        [Required]
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
