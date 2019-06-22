using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase.Domain
{
    public class MovieUser
    {
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        public string UserId { get; set; }
        public MovieDatabaseUser User { get; set; }
    }
}
