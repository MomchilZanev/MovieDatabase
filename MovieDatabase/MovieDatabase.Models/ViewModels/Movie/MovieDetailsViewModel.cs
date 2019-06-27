using System;
using System.Collections.Generic;

namespace MovieDatabase.Models.ViewModels.Movie
{
    public class MovieDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }

        public int Length { get; set; }

        public string Genre { get; set; }

        public string CoverImageLink { get; set; }

        public double Rating { get; set; }

        public string Director { get; set; }

        public List<MovieCastViewModel> Cast { get; set; }

        public List<MovieReviewViewModel> Reviews { get; set; }
    }
}
