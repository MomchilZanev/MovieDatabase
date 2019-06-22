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
            this.WatchlistedMovies = new HashSet<MovieUser>();
            this.WatchlistedTVShows = new HashSet<TVShowUser>();
        }

        public string AvatarLink { get; set; }

        public ICollection<MovieReview> MovieReviews { get; set; }

        public ICollection<SeasonReview> SeasonReviews { get; set; }

        //Watchlist
        public ICollection<MovieUser> WatchlistedMovies { get; set; }
        public ICollection<TVShowUser> WatchlistedTVShows { get; set; }
    }
}
