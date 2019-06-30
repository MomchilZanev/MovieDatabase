using System;
using System.Collections.Generic;

namespace MovieDatabase.Models.ViewModels.Artist
{
    public class ArtistDetailsViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Biography { get; set; }

        public string PhotoLink { get; set; }

        public List<string> MoviesDirected { get; set; }

        public List<string> TVShowsCreated { get; set; }

        public Dictionary<string, string> MovieRoles { get; set; }

        public Dictionary<string, string> SeasonRoles { get; set; }
    }
}
