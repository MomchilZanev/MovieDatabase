using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Domain
{
    public class MovieDatabaseUser : IdentityUser
    {
        public MovieDatabaseUser()
        {
            MovieReviews = new HashSet<MovieReview>();
            SeasonReviews = new HashSet<SeasonReview>();
            WatchlistedMovies = new HashSet<MovieUser>();
            WatchlistedTVShows = new HashSet<TVShowUser>();
        }

        [Required]
        public string AvatarLink { get; set; }

        public virtual ICollection<MovieReview> MovieReviews { get; set; }
        public virtual ICollection<SeasonReview> SeasonReviews { get; set; }

        public virtual ICollection<MovieUser> WatchlistedMovies { get; set; }
        public virtual ICollection<TVShowUser> WatchlistedTVShows { get; set; }
    }
}
