using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieDatabase.Domain
{
    public class MovieDatabaseUser : IdentityUser
    {
        public MovieDatabaseUser()
        {
            this.MovieReviews = new HashSet<MovieReview>();
            this.SeasonReviews = new HashSet<SeasonReview>();
        }

        public string AvatarLink { get; set; }

        public ICollection<MovieReview> MovieReviews { get; set; }

        public ICollection<SeasonReview> SeasonReviews { get; set; }

        public Watchlist Watchlist { get; set; }
    }
}
