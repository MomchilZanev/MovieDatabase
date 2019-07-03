using System;
using System.Collections.Generic;

namespace MovieDatabase.Models.ViewModels.TVShow
{
    public class SeasonDetailsViewModel
    {
        public string Id { get; set; }

        public int SeasonNumber { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Episodes { get; set; }

        public int LengthPerEpisode { get; set; }

        public double Rating { get; set; }

        public string TVShow { get; set; }

        public List<SeasonCastViewModel> Cast { get; set; }

        public List<SeasonReviewViewModel> Reviews { get; set; }

        public bool IsReviewedByCurrentUser { get; set; }
    }
}
