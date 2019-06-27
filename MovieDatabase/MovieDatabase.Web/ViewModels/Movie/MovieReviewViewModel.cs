using System;

namespace MovieDatabase.Web.ViewModels.Movie
{
    public class MovieReviewViewModel
    {
        public string Movie { get; set; }

        public string User { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
