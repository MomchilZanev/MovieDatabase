using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.ViewModels.Review
{
    public class ReviewAllViewModel
    {
        public string Item { get; set; }

        public string User { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
