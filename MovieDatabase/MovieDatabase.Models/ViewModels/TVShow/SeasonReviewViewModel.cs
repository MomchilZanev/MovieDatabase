using System;

namespace MovieDatabase.Models.ViewModels.TVShow
{
    public class SeasonReviewViewModel
    {
        public int Season { get; set; }

        public string User { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
