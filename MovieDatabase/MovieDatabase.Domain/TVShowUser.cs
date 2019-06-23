namespace MovieDatabase.Domain
{
    public class TVShowUser
    {
        public string TVShowId { get; set; }
        public virtual TVShow TVShow { get; set; }

        public string UserId { get; set; }
        public virtual MovieDatabaseUser User { get; set; }
    }
}
