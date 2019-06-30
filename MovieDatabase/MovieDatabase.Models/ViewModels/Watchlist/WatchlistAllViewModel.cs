using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Models.ViewModels.Watchlist
{
    public class WatchlistAllViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }

        public string CoverImageLink { get; set; }

        public double Rating { get; set; }

        public Class Type { get; set; }
    }

    public enum Class
    {
        Movie = 1,
        TVShow = 2
    }
}
