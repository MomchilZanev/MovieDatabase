using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class Announcement
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Creator { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public string ImageLink { get; set; }

        [Required]
        public string OfficialArticleLink { get; set; }
    }
}
