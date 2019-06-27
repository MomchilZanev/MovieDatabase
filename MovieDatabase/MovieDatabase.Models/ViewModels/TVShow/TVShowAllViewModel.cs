using System;

namespace MovieDatabase.Models.ViewModels.TVShow
{
    public class TVShowAllViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverImageLink { get; set; }

        public double Rating { get; set; }

        public DateTime FirstSeasonReleaseDate { get; set; }

        public int TotalReviews { get; set; }
    }
}
