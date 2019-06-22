using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Announcement
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageLink { get; set; }
    }
}
