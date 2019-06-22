using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Domain;

namespace MovieDatabase.Data
{
    public class MovieDatabaseDbContext : IdentityDbContext<MovieDatabaseUser>
    {
        public MovieDatabaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieReview> MovieReviews { get; set; }
        public DbSet<MovieRole> MovieRoles { get; set; }
        public DbSet<MovieUser> MovieUsers { get; set; }
        public DbSet<Season> Seasons { get; set; }        
        public DbSet<SeasonReview> SeasonReviews { get; set; }
        public DbSet<SeasonRole> SeasonRoles { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<TVShowUser> TVShowUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieRole>(movieRole =>
            {
                movieRole
                    .HasKey(k => new { k.MovieId, k.ArtistId });

                movieRole
                    .HasOne(m => m.Movie)
                    .WithMany(c => c.Cast)
                    .OnDelete(DeleteBehavior.Restrict);

                movieRole
                    .HasOne(a => a.Artist)
                    .WithMany(mr => mr.MovieRoles)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MovieReview>(movieReview =>
            {
                movieReview
                    .HasKey(k => new { k.MovieId, k.UserId });

                movieReview
                    .HasOne(m => m.Movie)
                    .WithMany(r => r.Reviews)
                    .OnDelete(DeleteBehavior.Restrict);

                movieReview
                    .HasOne(u => u.User)
                    .WithMany(mr => mr.MovieReviews)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MovieUser>(movieUser =>
            {
                movieUser
                    .HasKey(k => new { k.MovieId, k.UserId });

                movieUser
                    .HasOne(m => m.Movie)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                movieUser
                    .HasOne(u => u.User)
                    .WithMany(w => w.WatchlistedMovies)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SeasonRole>(seasonRole =>
            {
                seasonRole
                    .HasKey(k => new { k.SeasonId, k.ArtistId });

                seasonRole
                    .HasOne(s => s.Season)
                    .WithMany(c => c.Cast)
                    .OnDelete(DeleteBehavior.Restrict);

                seasonRole
                    .HasOne(a => a.Artist)
                    .WithMany(sr => sr.SeasonRoles)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SeasonReview>(seasonReview =>
            {
                seasonReview
                    .HasKey(k => new { k.SeasonId, k.UserId });

                seasonReview
                    .HasOne(s => s.Season)
                    .WithMany(sr => sr.Reviews)
                    .OnDelete(DeleteBehavior.Restrict);

                seasonReview
                    .HasOne(u => u.User)
                    .WithMany(sr => sr.SeasonReviews)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TVShowUser>(tvShowUser =>
            {
                tvShowUser
                    .HasKey(k => new { k.TVShowId, k.UserId });

                tvShowUser
                    .HasOne(m => m.TVShow)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                tvShowUser
                    .HasOne(u => u.User)
                    .WithMany(w => w.WatchlistedTVShows)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
