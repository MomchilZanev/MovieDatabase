using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieDatabase.Domain;

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
                         options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<MovieDatabaseUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<MovieDatabaseDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {                
                using (var context = serviceScope.ServiceProvider.GetRequiredService<MovieDatabaseDbContext>())
                {
                    //Bad Temporary DB seeding for Testing
                    context.Database.Migrate();
                    if (!context.Artists.Any())
                    {
                        var artist1 = new Artist
                        {
                            FullName = "Arist 1",
                            BirthDate = DateTime.Now,
                            Biography = "Some stuff"
                        };
                        context.Artists.Add(artist1);
                        var artist2 = new Artist
                        {
                            FullName = "Arist 2",
                            BirthDate = DateTime.Now,
                            Biography = "Biography"
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
                                    Artist = context.Artists.FirstOrDefault(x => x.FullName == "Arist 2"),
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
    }
}
