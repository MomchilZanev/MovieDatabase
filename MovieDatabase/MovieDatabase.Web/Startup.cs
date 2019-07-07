using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MovieDatabaseDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                        .UseLazyLoadingProxies());

            services.AddDefaultIdentity<MovieDatabaseUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
				.AddDefaultUI(UIFramework.Bootstrap4)
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MovieDatabaseDbContext>();

            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<ITVShowService, TVShowService>();
            services.AddTransient<IWatchlistService, WatchlistService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IAvatarService, AvatarService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<MovieDatabaseDbContext>())
                {
                    SeedDatabase(context);
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SeedDatabase(MovieDatabaseDbContext context)
        {
            //Bad Temporary DB seeding for Testing
            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                var adminRole = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                };

                var userRole = new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                };

                context.Roles.Add(adminRole);
                context.Roles.Add(userRole);

                context.SaveChanges();
            }

            if (!context.Artists.Any())
            {
                var artist1 = new Artist
                {
                    FullName = "Artist 1",
                    BirthDate = DateTime.Now,
                    Biography = "Some stuff",
                    PhotoLink = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/98/Al_Pacino.jpg/220px-Al_Pacino.jpg",
                };
                context.Artists.Add(artist1);
                var artist2 = new Artist
                {
                    FullName = "Artist 2",
                    BirthDate = DateTime.Now,
                    Biography = "Biography",
                    PhotoLink = "https://img.discogs.com/OTJ2a6_ebzFgKzJwiauHQocwJU8=/fit-in/300x300/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/A-259648-1551034602-5617.jpeg.jpg",
                };
                context.Artists.Add(artist2);

                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                var movie1 = new Movie
                {
                    Name = "Movie 1",
                    ReleaseDate = DateTime.UtcNow,
                    Description = "Some Desc.",
                    Length = 144,
                    Genre = new Genre { Name = "Genre 1" },
                    Director = context.Artists.First(),
                    Cast = new List<MovieRole>()
                    {
                        new MovieRole
                        {
                            Artist = context.Artists.FirstOrDefault(x => x.FullName == "Artist 2"),
                            CharacterPlayed = "Some Character"
                        }
                    },
                    Reviews = new List<MovieReview>()
                    {
                        new MovieReview
                        {
                            User = context.Users.FirstOrDefault(x => x.UserName == "Pesho"),
                            Content = "Nice movie",
                            Rating = 8,
                            Date = DateTime.UtcNow
                        }
                    }
                };
                context.Movies.Add(movie1);
                context.SaveChanges();
            }

            if (!context.Announcements.Any())
            {
                var announcement1 = new Announcement
                {
                    Creator = "Pesho",
                    Title = "Some title",
                    Content = "sahfgakjdgsdjgvfoeg",
                    ImageLink = "https://cdn-images-1.medium.com/max/1600/1*ZVYpEAxnObj7kWNnyKr_nQ.jpeg",
                    Date = DateTime.UtcNow,
                    OfficialArticleLink = "Tuk"
                };
                context.Announcements.Add(announcement1);

                var announcement2 = new Announcement
                {
                    Creator = "Gosho",
                    Title = "Other title",
                    Content = "asfasfabfytrohiewrobiweiobvgioewrhgviwelsufvaouse",
                    ImageLink = "https://publicrelationssydney.com.au/wp-content/uploads/2012/09/Breaking-news-loudspeaker.jpg",
                    Date = DateTime.UtcNow,
                    OfficialArticleLink = "Tam"
                };
                context.Announcements.Add(announcement2);
                context.SaveChanges();
            }

            if (!context.TVShows.Any())
            {
                var tvShow1 = new TVShow
                {
                    Name = "TVShow 1",
                    Description = "Some Desc.",
                    Genre = new Genre { Name = "Genre 2" },
                    Creator = context.Artists.First(),
                    CoverImageLink = "https://upload.wikimedia.org/wikipedia/en/thumb/0/0f/Dexter_Season_8_promotional_poster.jpeg/220px-Dexter_Season_8_promotional_poster.jpeg",
                };

                var season = new Season
                {
                    SeasonNumber = 1,
                    Episodes = 12,
                    LengthPerEpisode = 45,
                    ReleaseDate = DateTime.UtcNow,
                    Cast = new List<SeasonRole>()
                    {
                        new SeasonRole
                        {
                            Artist = context.Artists.Find("376b7620-6dbd-4870-a230-78b793ddd90e"),
                            CharacterPlayed = "Cool Character"
                        }
                    },
                    Reviews = new List<SeasonReview>()
                    {
                        new SeasonReview
                        {
                            User = context.Users.FirstOrDefault(x => x.UserName == "Gosho"),
                            Content = "Good TvShow",
                            Rating = 7,
                            Date = DateTime.UtcNow
                        }
                    },
                    TVShow = tvShow1,
                };

                context.Seasons.Add(season);
                context.TVShows.Add(tvShow1);
                context.SaveChanges();
            }
        }
    }
}
