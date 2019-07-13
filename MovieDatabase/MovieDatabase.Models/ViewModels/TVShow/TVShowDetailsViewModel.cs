using System;
using System.Collections.Generic;

namespace MovieDatabase.Models.ViewModels.TVShow
{
    public class TVShowDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime FirstAired { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public string CoverImageLink { get; set; }

        public string TrailerLink { get; set; }

        public double Rating { get; set; }

        public string Creator { get; set; }

        public Dictionary<string, int> Seasons { get; set; }

        public int Episodes { get; set; }
    }
}
