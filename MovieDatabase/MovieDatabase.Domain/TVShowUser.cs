using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Domain
{
    public class TVShowUser
    {
        public string TVShowId { get; set; }
        public TVShow TVShow { get; set; }

        public string UserId { get; set; }
        public MovieDatabaseUser User { get; set; }
    }
}
