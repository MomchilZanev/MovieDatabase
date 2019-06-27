using System;

namespace MovieDatabase.Web.ViewModels.TVShow
{
    public class TVShowAllViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverImageLink { get; set; }

        public double Rating { get; set; }

        public DateTime? ReleaseDate { get; set; } //TODO fix code quality (Release Date is not intuitive because it is not in database)

        public int TotalReviews { get; set; }
    }
}
