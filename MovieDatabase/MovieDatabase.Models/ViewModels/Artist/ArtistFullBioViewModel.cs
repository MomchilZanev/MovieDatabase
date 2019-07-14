using System;

namespace MovieDatabase.Models.ViewModels.Artist
{
    public class ArtistFullBioViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Biography { get; set; }

        public string PhotoLink { get; set; }
    }
}
