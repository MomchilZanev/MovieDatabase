using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services;
using MovieDatabase.Services.Contracts;
using MovieDatabase.Web.AutoMapperProfiles;
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

        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<ITVShowService, TVShowService>();
            services.AddTransient<IWatchlistService, WatchlistService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IAvatarService, AvatarService>();
            services.AddTransient<IUserService, UserService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SeedDatabase(MovieDatabaseDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                var adminRole = new IdentityRole
                {
                    Name = GlobalConstants.adminRoleName,
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

            if (!context.Announcements.Any())
            {
                var announcement1 = new Announcement
                {
                    Creator = "Libby Hill",
                    Title = "‘Game of Thrones’ Destroys Single Season Emmy Nomination Record",
                    Content = "At least “Game of Thrones'” final season wasn’t a colossal disappointment with regards to Emmy nominations.HBO’s dragon drama left its final season’s naysayers in the show’s fiery wake at the Emmy nominations,scoring a mind - boggling 32 nominations,the most nominations for a single season of any show in history. It was unclear heading into nominations whether industry voters would be as disappointed with the final season of the fantasy behemoth as some fans were, but Tuesday’s haul definitively proved otherwise. Beyond nabbing its eighth nomination for outstanding drama series, “Game of Thrones” earned several other notable nominations, including one for writing, three for direction, plus an astonishing 10 total acting mentions, including Kit Harington for lead actor, Emilia Clarke for lead actress, Gwendoline Christie, Lena Headey, Sophie Turner, and Maisie Williams for supporting actress, Alfie Allen, Nikolaj Coster - Waldau, and Peter Dinklage for supporting actor, and",
                    ImageLink = "https://m.media-amazon.com/images/M/MV5BMjA5NzA5NjMwNl5BMl5BanBnXkFtZTgwNjg2OTk2NzM@._V1_.jpg",
                    Date = DateTime.Parse("16 July 2019"),
                    OfficialArticleLink = "https://www.indiewire.com/2019/07/game-of-thrones-single-season-emmy-nomination-record-1202158251/"
                };

                context.Announcements.Add(announcement1);

                var announcement2 = new Announcement
                {
                    Creator = "Dave McNary",
                    Title = "Film News Roundup: ‘Apollo 11’ Re-Release Set for Moon Landing Anniversary",
                    Content = "In today’s film news roundup, Neon is re-releasing “Apollo 11”; “Sesame Street” gets moved; “Supersize Me 2” is set for Sept. 13; Will Ropp gets a “Silk Road” deal; and Apple makes a movie deal. Re - launch Neon will re - release Todd Douglas Miller’s documentary “Apollo 11” in theaters on July 20, the 50th anniversary of the first moon landing. The run will also include weeklong engagements and special one - off showings in more than 100 theaters across top markets including New York, Los Angeles, San Francisco, Boston, Philadelphia, Chicago, Dallas, Houston, Atlanta and Miami. “Apollo 11,” which was crafted from a newly discovered trove of 65mm footage and more than 11, 000 hours of uncatalogued audio recordings, premiered at this year’s Sundance Film Festival where it won the editing prize.The film, which opened in theaters on March 1, will cross $9 million after the weekend’s anniversary re - launch and remains the top grossing documentary of the year.",
                    ImageLink = "https://m.media-amazon.com/images/M/MV5BMTYyMzEzNjI4M15BMl5BanBnXkFtZTgwODgxOTgyNzM@._V1_.jpg",
                    Date = DateTime.Parse("17 July 2019"),
                    OfficialArticleLink = "https://variety.com/2019/film/news/apollo-11-re-release-moon-landing-anniversary-1203269547/"
                };
                context.Announcements.Add(announcement2);
                context.SaveChanges();
            }

            if (!context.Artists.Any())
            {
                var artist1 = new Artist
                {
                    FullName = "Francis Ford Coppola",
                    BirthDate = DateTime.Parse("7 April 1939"),
                    Biography = @"Francis Ford Coppola was born in 1939 in Detroit, Michigan, but grew up in a New York suburb in a creative, supportive Italian-American family. His father, Carmine Coppola, was a composer and musician. His mother, Italia Coppola (née Pennino), had been an actress. Francis Ford Coppola graduated with a degree in drama from Hofstra University, and did graduate work at UCLA in filmmaking. He was training as assistant with filmmaker Roger Corman, working in such capacities as sound-man, dialogue director, associate producer and, eventually, director of Луди сънища (1963), Coppola's first feature film. During the next four years, Coppola was involved in a variety of script collaborations, including writing an adaptation of 'This Property is Condemned' by Tennessee Williams (with Fred Coe and Edith Sommer), and screenplays for Гори ли Париж? (1966) and Патън (1970), the film for which Coppola won a Best Original Screenplay Academy Award. In 1966, Coppola's 2nd film brought him critical acclaim and a Master of Fine Arts degree. In 1969, Coppola and George Lucas established American Zoetrope, an independent film production company based in San Francisco. The company's first project was Ти Ейч Екс 1138 (1971), produced by Coppola and directed by Lucas. Coppola also produced the second film that Lucas directed, Американски графити (1973), in 1973. This movie got five Academy Award nominations, including one for Best Picture. In 1971, Coppola's film Кръстникът (1972) became one of the highest-grossing movies in history and brought him an Oscar for writing the screenplay with Mario Puzo The film was a Best Picture Academy Award-winner, and also brought Coppola a Best Director Oscar nomination. Following his work on the screenplay for Великият Гетсби (1974), Coppola's next film was Разговорът (1974), which was honored with the Golden Palm Award at the Cannes Film Festival, and brought Coppola Best Picture and Best Original Screenplay Oscar nominations. Also released that year, Кръстникът II (1974), rivaled the success of Кръстникът (1972), and won six Academy Awards, bringing Coppola Oscars as a producer, director and writer. Coppola then began work on his most ambitious film, Апокалипсис сега (1979), a Vietnam War epic that was inspired by Joseph Conrad's Сърцето на мрака (1993). Released in 1979, the acclaimed film won a Golden Palm Award at the Cannes Film Festival, and two Academy Awards. Also that year, Coppola executive produced the hit Чeрният жребец (1979). With George Lucas, Coppola executive produced Кагемуша: Сянката на воина (1980), directed by Akira Kurosawa, and Мишима (1985), directed by Paul Schrader and based on the life and writings of Yukio Mishima. Coppola also executive produced such films as The Escape Artist (1982), Хамет (1982) Завръщането на черния жребец (1983), Муха на бара (1987), Вятър (1992), Тайната градина (1993), etc. He helped to make a star of his nephew,
                    Nicolas Cage.Personal tragedy hit in 1986 when his son Gio died in a boating accident.Francis Ford Coppola is one of America's most erratic, energetic and controversial filmmakers.",
                    PhotoLink = "https://m.media-amazon.com/images/M/MV5BMTM5NDU3OTgyNV5BMl5BanBnXkFtZTcwMzQxODA0NA@@._V1_.jpg",
                };
                context.Artists.Add(artist1);

                var artist2 = new Artist
                {
                    FullName = "Alfredo James Pacino",
                    BirthDate = DateTime.Parse("25 April 1940"),
                    Biography = @"One of the greatest actors in all of film history, Al Pacino established himself during one of cinema's most vibrant decades, the 1970s, and has become an enduring and iconic figure in the world of American movies.

Alfredo James Pacino was born on April 25, 1940 in Manhattan, New York City, to an Italian-American family. His parents, Rose (Gerardi) and Sal Pacino, divorced when he was young. His mother moved them into his grandparents' house. Pacino found himself often repeating the plots and voices of characters he had seen in the movies, one of his favorite activities. Bored and unmotivated in school, the young Al Pacino found a haven in school plays, and his interest soon blossomed into a full-time career. Starting on the stage, he went through a lengthy period of depression and poverty, sometimes having to borrow bus fare to succeed to auditions. He made it into the prestigious Actors Studio in 1966, studying under legendary acting coach Lee Strasberg, creator of the Method Approach that would become the trademark of many 1970s-era actors.

After appearing in a string of plays in supporting roles, Pacino finally succeeded with Israel Horovitz's 'The Indian Wants the Bronx', winning an Obie Award for the 1966-67 season. That was followed by a Tony Award for 'Does the Tiger Wear a Necktie ? '. His first feature films made little departure from the gritty realistic stage performances that earned him respect: he played a drug addict in Паника в Парка на дрогата (1971) after his film debut in Me, Natalie (1969). What came next would change his life forever. The role of Michael Corleone in Кръстникът (1972) was one of the most sought-after of the time: Robert Redford, Warren Beatty, Jack Nicholson, Ryan O'Neal, Robert De Niro and a host of others either wanted it or were mentioned for it, but director Francis Ford Coppola had his heart set on the unknown Italian Pacino for the role, although pretty much everyone else--from the studio to the producers to some of the cast members--did not want him.

Though Coppola won out through slick persuasion,
                    Pacino was in constant fear of being fired during the hellish shoot.Much to his(and Coppola's) relief, the film was a monster hit that did wonders for everyone's career, including Pacino's, and earned him his first Academy Award nomination for Best Supporting Actor. However, instead of taking on easier projects for the big money he could now command, Pacino threw his support behind what he considered tough but important films, such as the true-life crime drama Серпико (1973) and the tragic real-life bank robbery film Кучешки следобед (1975). He opened eyes around the film world for his brave choice of roles, and he was nominated three consecutive years for the 'Best Actor' Academy Award. He faltered slightly with Боби Диърфийлд (1977), but regained his stride with И справедливост за всички (1979), for which he received another Academy Award nomination for Best Actor. Unfortunately, this would signal the beginning of a decline in his career, which produced such critical and commercial flops as Фатален партньор (1980) and Автора! Автора! (1982).

Pacino took on another vicious gangster role and cemented his legendary status in the ultra - violent cult film Белязаният(1983), but a monumental mistake was about to follow.Революция(1985) endured an endless and seemingly cursed shoot in which equipment was destroyed, weather was terrible, and Pacino became terribly sick with pneumonia.Constant changes in the script also further derailed a project that seemed doomed from the start anyway.The Revolutionary War film is considered one of the worst films ever, not to mention one of the worst of his career, resulted in his first truly awful reviews and kept him off the screen for the next four years.Returning to the stage, Pacino has done much to give back and contribute to the theatre, which he considers his first love.He directed a film, The Local Stigmatic(1990), but it remains unreleased.He lifted his self - imposed exile with the striking Море от любов(1989) as a hard - drinking policeman.This marked the second phase of Pacino's career, being the first to feature his now famous dark, owl eyes and hoarse, gravelly voice.

Returning to the Corleones, Pacino made Кръстникът III(1990) and earned raves for his first comedic role in the colorful adaptation Дик Трейси(1990).This earned him another Academy Award nomination for Best Supporting Actor, and two years later he was nominated for Гленгари Глен Рос(1992).He went into romantic mode for Франки и Джони(1991).In 1992, he finally won the Academy Award for Best Actor for his amazing performance in Усещане за жена(1992).A mixture of technical perfection(he plays a blind man) and charisma, the role was tailor - made for him, and remains a classic.The next few years would see Pacino becoming more comfortable with acting and movies as a business, turning out great roles in great films with more frequency and less of the demanding personal involvement of his wilder days.Пътят на Карлито(1993) proved another gangster classic, as did the epic crime drama Жега(1995) directed by Michael Mann and co - starring Robert De Niro, although they only had a few scenes together.He returned to the director's chair for the highly acclaimed and quirky Shakespeare adaptation Аз, Ричард (1996). Кметството (1996), Дони Браско (1997) and Адвокат на Дявола (1997) all came out in this period. Reteaming with Mann and then Oliver Stone, he gave two commanding performances in Вътрешен човек (1999) and Всяка една неделя (1999).

In the 2000s, Pacino starred in a number of theatrical blockbusters, including Бандата на Оушън 3(2007), but his choice in television roles(the vicious Roy Cohn in the HBO miniseries Angels in America(2003) and his sensitive portrayal of Jack Kevorkian, in the television movie Не познавате Джак(2010)) are reminiscent of the bolder choices of his early career.Each television project garnered him an Emmy Award for Outstanding Lead Actor in a Miniseries or a Movie.

In his personal life, Pacino is one of Hollywood's most enduring and notorious bachelors, having never been married. He has a daughter, Julie Marie, with acting teacher Jan Tarrant, and a set of twins with former longtime girlfriend Beverly D'Angelo.His romantic history includes Veruschka von Lehndorff, Jill Clayburgh, Debra Winger, Tuesday Weld, Marthe Keller, Carmen Cervera, Kathleen Quinlan, Lyndall Hobbs, Penelope Ann Miller, and a two - decade intermittent relationship with 'Godfather' co - star Diane Keaton.He currently lives with Argentinian actress Lucila Solá, who is 36 years his junior.

With his intense and gritty performances, Pacino was an original in the acting profession.His Method approach would become the process of many actors throughout time, and his unbeatable number of classic roles has already made him a legend among film buffs and all aspiring actors and directors.His commitment to acting as a profession and his constant screen dominance has established him as one of the movies' true legends.

Pacino has never abandoned his love for the theater, and Shakespeare in particular, having directed the Shakespeare adaptation Аз, Ричард(1996) and played Shylock in Венецианският търговец(2004).",
                    PhotoLink = "https://i.pinimg.com/originals/a2/0a/40/a20a4016bac38ad227033884288f932c.jpg",
                };
                context.Artists.Add(artist2);

                var artist3 = new Artist
                {
                    FullName = "Brian De Palma",
                    BirthDate = DateTime.Parse("11 September 1940"),
                    Biography = @"Brian De Palma is the son of a surgeon. He studied physics but at the same time felt his dedication for the movies and made some short films. After seven independent productions he had his first success with Sisters (1972) and his voyeuristic style. Restlessly he worked on big projects with the script writers Paul Schrader, John Farris and Oliver Stone. He also filmed a novel of Stephen King: Carrie (1976). Another important film was The Untouchables (1987) with a script by David Mamet adapted from the TV series.
- IMDb Mini Biography By: Volker Boehm

Brian De Palma is one of the well-known directors who spear-headed the new movement in Hollywood during the 1970s. He is known for his many films that go from violent pictures, to Hitchcock-like thrillers.

Born on the 11th of September in 1940, De Palma was born in New Jersey in an American-Italian family. Originally entering university as a physics student, de Palma became attracted to films after seeing such classics as Citizen Kane (1941). Enrolling in Sarah Lawrence College, he found lasting influences from such varied teachers as Alfred Hitchcock and Andy Warhol.

At first, his films comprised of such black-and-white films as To Bridge This Gap (1969). He then discovered a young actor whose fame would influence Hollywood forever. In 1968, de Palma made the comedic film Greetings (1968) starring Robert de Niro in his first ever credited film role. The two followed up immediately with the film The Wedding Party (1969) and Hi, Mom! (1970).

After making such small-budget thrillers such as Sisters (1972) and Obsession (1976), De Palma was offered the chance to direct a film based on the Stephen King novel 'Carrie'. The story deals with a tormented teenage girl who finds she has the power of telekinesis. The film starred Sissy Spacek, Piper Laurie and John Travolta, and was for De Palma, a chance to try out the split screen technique for which he would later become famous.

Carrie(1976) was a massive success,
                    and earned the two lead females(Laurie and Spacek) Oscar nominations.The film was praised by most critics,
                    and De Palma's reputation was now permanently secured. He followed up this success with the horror film The Fury (1978), the comedic Home Movies (1979) (both these films featured Kirk Douglas, the crime film Dressed to Kill (1980), and another crime thriller entitled Blow Out (1981) starring John Travolta.

His next major success was the controversial,
                    ultra - violent film Scarface(1983).Written by Oliver Stone and starring Al Pacino,
                    the film concerned Cuban immigrant Tony Montana's rise to power in the United States through the drug trade. The film, while being a critical failure, was a major success commercially.

Moving on from Scarface (1983),
                    De Palma made two more movies before landing another one of his now - classics: The Untouchables(1987), starring old friend 'Robert de Niro' in the role of Chicago gangster Al Capone.Also starring in the film were Kevin Costner as the man who commits himself to bring Capone down, and Sean Connery, an old policeman who helps Costner's character to form a group known as the Untouchables. The film was one of de Palma's most successful films, earning Connery an Oscar, and gave Ennio Morricone a nomination for Best Score.


After The Untouchables(1987), De Palma made the Vietnam film Casualties of War(1989) starring Michael J.Fox and Sean Penn.The film focuses on a new soldier who is helpless to stop his dominating sergeant from kidnapping a Vietnamese girl with the help of the coerced members of the platoon.The film did reasonably well at the box office, but it was his next film that truly displayed the way he could make a hit and a disaster within a short time.The Bonfire of the Vanities(1990) starred a number of well - known actors such as Bruce Willis and Morgan Freeman, yet it was still a commercial flop and earned him two Razzie nominations.


But the roller coaster success that De Palma had gotten so far did not let him down.He made the horror film Raising Cain(1992), and the criminal drama Carlito's Way (1993) starring Al Pacino and Sean Penn. The latter film is about a former criminal just released from prison that is trying to avoid his past and move on. It was in the year 1996 that brought one of his most well-known movies. This was the suspense-filled Mission: Impossible (1996) starring Tom Cruise and Jon Voight.


Following up this film was the interesting but unsuccessful film Snake Eyes(1998) starring Nicolas Cage as a detective who finds himself in the middle of a murder scene at a boxing ring.De Palma continued on with the visually astounding but equally unsuccessful film Mission to Mars(2000) which earned him another Razzie nomination.He met failure again with the crime / thriller Femme Fatale(2002) the murder conspiracy The Black Dahlia(2006) and the controversial film Redacted(2007) which deals with individual stories from the war in Iraq.


Brian De Palma may be down for the moment, but if his box office history has taught us anything, it is that he always returns with a major success that is remembered for years and years afterwards.",
                    PhotoLink = "https://le-citazioni.it/media/authors/20579_brian-de-palma.jpeg",
                };
                context.Artists.Add(artist3);

                var artist4 = new Artist
                {
                    FullName = "Nic Pizzolatto",
                    BirthDate = DateTime.Parse("18 October 1975"),
                    Biography = @"An award-winning novelist and short-story writer, he is the author of the collection 'Between Here and the Yellow Sea' and the novel 'Galveston.' He is originally from Southwest Louisiana, and taught literature at several universities, including the University of Chicago, before going into screenwriting in 2010. His fiction has been translated into French, German, Spanish, Japanese and Italian.",
                    PhotoLink = "https://m.media-amazon.com/images/M/MV5BOTM2MDkxNzYzMl5BMl5BanBnXkFtZTgwMzg2MDY0NDE@._V1_.jpg",
                };
                context.Artists.Add(artist4);

                var artist5 = new Artist
                {
                    FullName = "Matthew David McConaughey",
                    BirthDate = DateTime.Parse("4 November 1969"),
                    Biography = @"American actor and producer Matthew David McConaughey was born in Uvalde, Texas. His mother, Mary Kathleen (McCabe), is a substitute school teacher originally from New Jersey. His father, James Donald McConaughey, was a Mississippi-born gas station owner who ran an oil pipe supply business. He is of Irish, Scottish, English, German, and Swedish descent. Matthew grew up in Longview, Texas, where he graduated from the local High School (1988). Showing little interest in his father's oil business, which his two brothers later joined, Matthew was longing for a change of scenery, and spent a year in Australia, washing dishes and shoveling chicken manure. Back to the States, he attended the University of Texas in Austin, originally wishing to be a lawyer. But, when he discovered an inspirational Og Mandino book 'The Greatest Salesman in the World' before one of his final exams, he suddenly knew he had to change his major from law to film.

He began his acting career in 1991,
                    appearing in student films and commercials in Texas and directed short films as Chicano Chariots(1992).Once, in his hotel bar in Austin,
                    he met the casting director and producer Don Phillips,
                    who introduced him to director Richard Linklater for his next project.At first, Linklater thought Matthew was too handsome to play the role of a guy chasing high school girls in his coming - of - age drama Dazed and Confused(1993), but cast him after Matthew grew out his hair and mustache.His character was initially in three scenes but the role grew to more than 300 lines as Linklater encouraged him to do some improvisations. In 1995, he starred in The Return of the Texas Chainsaw Massacre(1994), playing a mad bloodthirsty sadistic killer, opposite Renée Zellweger.

Shortly thereafter, moving to L.A., Matthew became a sensation with his performances in two high-profile 1996 films Lone Star(1996), where he portrayed killing suspected sheriff and in the film adaptation of John Grisham's novel A Time to Kill (1996), where he played an idealistic young lawyer opposite Sandra Bullock and Kevin Spacey. The actor was soon being hailed as one of the industry's hottest young leading man inspiring comparisons to actor Paul Newman.His following performances were Robert Zemeckis' Contact (1997) with Jodie Foster (the film was finished just before the death of the great astronomer and popularizer of space science Carl Sagan) and Steven Spielberg's Amistad(1997), a fact-based 1839 story about the rebellious African slaves. In 1998, he teamed again with Richard Linklater as one of the bank - robbing brothers in The Newton Boys(1998), set in Matthew's birthplace, Uvalde, Texas. During this time, he also wrote, directed and starred in the 20-minute short The Rebel (1998).

In 1999, he starred in the comedy Edtv(1999), about the rise of reality television, and in 2000, he headlined Jonathan Mostow's U-571 (2000), portraying officer Lt. Tyler, in a WW II story of the daring mission of American submariners trying to capture the Enigma cipher machine.

In the 2000s, he became known for starring in romantic comedies, such as The Wedding Planner(2001), opposite Jennifer Lopez, and How to Lose a Guy in 10 Days(2003), in which he co - starred with Kate Hudson.He played Denton Van Zan, an American warrior and dragons hunter in the futuristic thriller Reign of Fire(2002), where he co - starred with Christian Bale.In 2006, he starred in the romantic comedy Failure to Launch(2006), and later as head coach Jack Lengyel in We Are Marshall(2006), along with Matthew Fox.In 2008, he played treasure hunter Benjamin 'Finn' Finnegan in Fool's Gold (2008), again with Kate Hudson. After playing Connor Mead in Ghosts of Girlfriends Past (2009), co-starring with Jennifer Garner, McConaughey took a two year hiatus to open different opportunities in his career. Since 2010, he has moved away from romantic comedies.

That change came in 2011, in his first movie after that pause, when he portrayed criminal defense attorney Mickey Haller in The Lincoln Lawyer(2011), that operates mostly from the back seat of his Lincoln car.After this performance that was considered one of his best until then, Matthew played other iconic characters as district attorney Danny Buck Davidson in Bernie(2011), the wild private detective 'Killer' Joe Cooper in Killer Joe(2011), Mud in Mud(2012), reporter Ward Jensen in The Paperboy(2012), male stripper club owner Dallas in Magic Mike(2012), starring Channing Tatum.McConaughey's career certainly reached it's prime, when he played HIV carrier Ron Woodroof in the biographical drama Dallas Buyers Club(2013), shot in less than a month.For his portrayal of Ron, Matthew won the Best Actor in the 86th Academy Awards, as well as the Golden Globe Award for Best Actor, among other awards and nominations.The same year, he also appeared in Martin Scorsese's The Wolf of Wall Street (2013). In 2014, he starred in HBO's True Detective (2014), as detective Rustin Cohle, whose job is to investigate with his partner Martin Hart, played by Woody Harrelson, a gruesome murder that happened in his little town in Louisiana.The series was highly acclaimed by critics winning 4 of the 7 categories it was nominated at the 66th Primetime Emmy Awards; he also won a Critics' Choice Award for the role.

Also in 2014, Matthew starred in Christopher Nolan's sci-fi film Interstellar (2014), playing Cooper, a former NASA pilot.",
                    PhotoLink = "https://upload.wikimedia.org/wikipedia/commons/8/8e/Matthew_McConaughey_-_Goldene_Kamera_2014_-_Berlin.jpg",
                };
                context.Artists.Add(artist5);

                var artist6 = new Artist
                {
                    FullName = "Colin James Farrell",
                    BirthDate = DateTime.Parse("31 May 1976"),
                    Biography = @"Colin Farrell is one of Ireland's biggest stars in Hollywood and abroad. His film presence has been filled with memorable roles that range from an inwardly tortured hit man, to an adventurous explorer, a determined-but-failing writer, and the greatest military leader in history.

Farrell was born on May 31, 1976 in Castleknock, Dublin, Ireland, to Rita (Monaghan) and Eamon Farrell. His father and uncle were both professional athletes, and for a while, it looked like Farrell would follow in their footsteps. Farrell auditioned for a part in the Irish Boy Band, Boyzone, but it didn't work out. After dropping out of the Gaiety School of Acting, Farrell was cast in Ballykissangel (1996), a BBC television drama. 'Ballykissangel' was not his first role on screen. Farrell had previously been in The War Zone (1999), directed by Tim Roth and had appeared in the independent film Drinking Crude (1997). Farrell was soon to move on to bigger things.

Exchanging his usually thick Dublin accent for a light Texas drawl, Farrell acted in the gritty Tigerland(2000), directed by Joel Schumacher.Starring Farrell amongst a number of other budding young actors, the film portrays a group of new recruits being trained for the war in Vietnam.Farrell played the arrogant soldier Boz, drafted into the army and completely spiteful of authority.The film was praised by critics, but did not make much money at the box office.It was Farrell's first big role on film, and certainly not his last. Farrell followed up with American Outlaws (2001), where he played the notorious outlaw Jesse James with Scott Caan, son of legendary actor James Caan, in the role of Cole Younger. The film was a box office flop and failure with the critics. Immediately, Farrell returned to the war drama film that had made him famous. Co-starring in the war film Hart's War(2002) opposite Bruce Willis, Farrell played the young officer captured by the enemy.The film was another failure.Farrell struck gold when he was cast in the Steven Spielberg film Minority Report(2002) that same year.Set in a futuristic time period, Farrell played the character Danny Witwer, a young member of the Justice Department who is sent after Tom Cruise's character. The film was a smash hit, and praised by critics.

Farrell continued this success when he reunited with Joel Schumacher on the successful thriller Phone Booth(2002).Farrell played the role of the victim who is harassed by an unseen killer(Kiefer Sutherland) and is made to reveal his sins to the public. 2003 was a big year for Farrell.He starred in the crime thriller The Recruit(2003) as a young CIA man mentored by an older CIA veteran(Al Pacino). Pacino later stated that Farrell was the best actor of his generation.Farrell certainly continued to be busy that year with Daredevil (2003), which actually allowed him to keep his thick Irish accent.The film was another success for Farrell, as was the crime film S.W.A.T. (2003) where Farrell starred opposite Samuel L.Jackson and LL Cool J.Farrell also acted in the Irish black comedy film Intermission (2003) and appeared another Irish film Veronica Guerin(2003) which reunited him with Joel Schumacher once again.The following year, Farrell acted in what is his most infamous film role yet: the title role in the mighty Oliver Stone film epic Alexander (2004), which is a character study of Alexander the Great as he travels across new worlds and conquers all the known world before him.Farrell donned a blond wig and retained his Irish accent, and gave a fine performance as Alexander.However, both he and the film were criticized.Despite being one of the highest grossing films internationally and doing a good job at the DVD sales, Farrell did not come out of the experience without a few hurts.Farrell attempted to rebound with his historical film The New World(2005). Reuniting with 'Alexander' star Christopher Plummer, and also acting with Christian Bale, Farrell played the brave explorer John Smith, who would make first contacts with the Native peoples.The film did not do well at the box office, though critics praised the film's stunning appearance and cinematography.

Farrell returned to act in Michael Mann's film Miami Vice (2006) alongside Jamie Foxx. The film was a film adaptation of the famous television series, and did reasonably well at the box office. Farrell also acted in Ask the Dust (2006) with Salma Hayek and Donald Sutherland, though the film did not receive much distribution. The next year, Farrell acted alongside Ewan McGregor in the Woody Allen film Cassandra's Dream (2007) which received mixed reviews from critics.Farrell followed up with the hilarious black comedy In Bruges (2008). Written and directed by Irish theatre director Martin McDonagh, the film stars Farrell and Brendan Gleeson as two Irish hit men whose latest assignment went wrong, leaving them to hide out in Bruges, Belgium.The film has been one of Farrell's most praised work, and he was nominated for a Golden Globe. As well as In Bruges (2008), Farrell acted alongside Edward Norton in the crime film Pride and Glory (2008) which was not as successful as the former film. As well as working with charity, and speaking at the Special Olympics World Games in 2007, he has donated his salary for Terry Gilliam's The Imaginarium of Doctor Parnassus(2009) to Heath Ledger's little daughter (who was left nothing in a will that had not been updated in time). Ledger had originally been cast in the film and was replaced by Farrell, Johnny Depp and Jude Law. The film was a critical and financial success, and Farrell also played a small role in Crazy Heart (2009) which had the Dubliner playing a country singer. Farrell even sang a few songs for the film's soundtrack.As well as those small roles, Farrell took the lead role in the war film Triage (2009). Farrell incredibly lost forty-four pounds to play the role of a war photographer who must come to terms with what he has experienced in Kurdistan.While the film was finely made, with excellent performances from all involved, the film has received almost no distribution.

Farrell's other leading role that year was in Neil Jordan's Irish film Ondine(2009). In recent years, he co-starred in the comedy horror film Fright Night(2011), the science fiction action film Total Recall(2012), both remakes, and McDonagh's second feature, and the black comedy crime film Seven Psychopaths (2012). Since the mid-2000s, Farrell has cleaned up his act, and far from being a Hollywood hell raiser and party animal, Farrell has shown himself to be a respectable and very talented actor.

He also starred in The Lobster(2015) and The Killing of a Sacred Deer(2017), both directed by Yorgos Lanthimos.For The Lobster he was nominated for an Golden Globe.",
                    PhotoLink = "https://m.media-amazon.com/images/M/MV5BMTg4NzM5NDk0MV5BMl5BanBnXkFtZTcwNzAzMTUxNw@@._V1_.jpg",
                };
                context.Artists.Add(artist6);

                var artist7 = new Artist
                {
                    FullName = "Mahershalalhashbaz Gilmore",
                    BirthDate = DateTime.Parse("16 February 1974"),
                    Biography = @"Mahershala Ali is fast becoming one of the freshest and most in-demand faces in Hollywood with his extraordinarily diverse skill set and wide-ranging background in film, television, and theater.

This past fall, Ali wrapped A24's Brad Pitt and Adele Romanski produced independent feature film, Moonlight, as well as reprising his role in The Hunger Games: Mockingjay - Part 2, the fourth and final installment in the critically and commercially acclaimed Hunger Games franchise, alongside Jennifer Lawrence, Donald Sutherland, and Julianne Moore. As District 13's Head of Security, 'Boggs' (Ali) guides and protects Katniss (Lawrence) through the final stages of the district's rebellion against the Capitol. Lionsgate released the film on November 20, 2015.

Ali will next star in Gary Ross's civil war era drama The Free State of Jones opposite Matthew McConaughey, Gugu Mbatha-Raw, and Keri Russell. STX Entertainment will release the film on May 13, 2016.

On television, Ali was recently cast in Netflix and Marvel Entertainment's Luke Cage in the role of Cornell 'Cottonmouth' Stokes. A Harlem nightclub owner, Stokes will become an unexpected foe in Luke's life when Stokes' criminal activities threaten Luke's world. Ali stars alongside Mike Colter, Rosario Dawson, and Alfre Woodard. The series will premiere on Netflix in 2016.

Ali can be seen on the award - winning Netflix original series House of Cards,
                    where he will reprise his fan - favorite role as a lobbyist and former press secretary Remy Danton for a fourth season in March 2016.

Ali's previous feature film credits include Derek Cianfrance's The Place Beyond the Pines opposite Ryan Gosling and Bradley Cooper, Wayne Kramer's Crossing Over starring Harrison Ford, John Sayles' Go For Sisters, and David Fincher's The Curious Case of Benjamin Button.


On television, he appeared opposite Julia Ormond in Lifetime's The Wronged Man for which he subsequently received an NAACP Nomination for Best Actor. Ali also had a large recurring role on Syfy's Alphas, as well as the role of Richard Tyler, a Korean War pilot, on the critically acclaimed drama The 4400 for three seasons.


On the stage, Ali appeared in productions of Blues for an Alabama Sky, The School for Scandal, A Lie of the Mind, A Doll's House, Monkey in the Middle, The Merchant of Venice, The New Place and Secret Injury, Secret Revenge. His additional stage credits include appearing in Washington, D.C. at the Arena Stage in the title role of The Great White Hope, and in The Long Walk and Jack and Jill. In February 2016, Ali will make his New York Broadway debut in Kenny Leon's Smart People, starring opposite Joshua Jackson.

Born in Oakland, California and raised in Hayward, Ali received his Bachelor of Arts degree in Mass Communications at St.Mary's College. He made his professional debut performing with the California Shakespeare Festival in Orinda, California. Soon after, he earned his Master's degree in acting from New York University's prestigious graduate program.",
                    PhotoLink = "https://m.media-amazon.com/images/M/MV5BNDg4OTczODE5Nl5BMl5BanBnXkFtZTcwMjgwMjA0Mg@@._V1_.jpg",
                };
                context.Artists.Add(artist7);

                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                var movie1 = new Movie
                {
                    Name = "Scarface",
                    ReleaseDate = DateTime.Parse("9 December 1983"),
                    Description = "Tony Montana manages to leave Cuba during the Mariel exodus of 1980. He finds himself in a Florida refugee camp but his friend Manny has a way out for them: undertake a contract killing and arrangements will be made to get a green card. He's soon working for drug dealer Frank Lopez and shows his mettle when a deal with Colombian drug dealers goes bad. He also brings a new level of violence to Miami. Tony is protective of his younger sister but his mother knows what he does for a living and disowns him. Tony is impatient and wants it all however, including Frank's empire and his mistress Elvira Hancock. Once at the top however, Tony's outrageous actions make him a target and everything comes crumbling down.",
                    Length = 170,
                    Genre = new Genre { Name = "Crime" },
                    Director = context.Artists.SingleOrDefault(a => a.FullName == "Brian De Palma"),
                    CoverImageLink = "https://www.virginmegastore.ae/medias/sys_master/root/hbd/h9b/8822240935966/Scarface-490422-Detail.jpg",
                    TrailerLink = "https://www.youtube.com/embed/vREl66xmXsE",
                    Cast = new List<MovieRole>()
                    {
                        new MovieRole
                        {
                            Artist = context.Artists.SingleOrDefault(x => x.FullName == "Alfredo James Pacino"),
                            CharacterPlayed = "Tony Montana"
                        }
                    },
                };
                context.Movies.Add(movie1);

                var movie2 = new Movie
                {
                    Name = "The Godfather",
                    ReleaseDate = DateTime.Parse("24 March 1972"),
                    Description = "The Godfather 'Don' Vito Corleone is the head of the Corleone mafia family in New York. He is at the event of his daughter's wedding. Michael, Vito's youngest son and a decorated WW II Marine is also present at the wedding. Michael seems to be uninterested in being a part of the family business. Vito is a powerful man, and is kind to all those who give him respect but is ruthless against those who do not. But when a powerful and treacherous rival wants to sell drugs and needs the Don's influence for the same, Vito refuses to do it. What follows is a clash between Vito's fading old values and the new ways which may cause Michael to do the thing he was most reluctant in doing and wage a mob war against all the other mafia families which could tear the Corleone family apart.",
                    Length = 175,
                    Genre = new Genre { Name = "Drama" },
                    Director = context.Artists.SingleOrDefault(a => a.FullName == "Francis Ford Coppola"),
                    CoverImageLink = "https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_.jpg",
                    TrailerLink = "https://www.youtube.com/embed/sY1S34973zA",
                    Cast = new List<MovieRole>()
                    {
                        new MovieRole
                        {
                            Artist = context.Artists.SingleOrDefault(x => x.FullName == "Alfredo James Pacino"),
                            CharacterPlayed = "Michael Corleone"
                        }
                    },
                };
                context.Movies.Add(movie2);

                context.SaveChanges();
            }

            if (!context.TVShows.Any())
            {
                var tvShow = new TVShow
                {
                    Name = "True Detective ",
                    Description = "Seasonal anthology series in which police investigations unearth the personal and professional secrets of those involved, both within and outside the law.",
                    Genre = new Genre { Name = "Mystery" },
                    Creator = context.Artists.SingleOrDefault(a => a.FullName == "Nic Pizzolatto"),
                    CoverImageLink = "http://es.web.img2.acsta.net/r_1280_720/pictures/14/01/17/13/00/493022.jpg",
                    TrailerLink = "https://www.youtube.com/embed/fVQUcaO4AvE"
                };

                var season1 = new Season
                {
                    SeasonNumber = 1,
                    Episodes = 8,
                    LengthPerEpisode = 60,
                    ReleaseDate = DateTime.Parse("12 January 2014"),
                    Cast = new List<SeasonRole>()
                    {
                        new SeasonRole
                        {
                            Artist = context.Artists.SingleOrDefault(a => a.FullName == "Matthew David McConaughey"),
                            CharacterPlayed = "Detective Rust Cohle"
                        }
                    },
                    TVShow = tvShow,
                };

                var season2 = new Season
                {
                    SeasonNumber = 2,
                    Episodes = 8,
                    LengthPerEpisode = 60,
                    ReleaseDate = DateTime.Parse("21 June 2015"),
                    Cast = new List<SeasonRole>()
                    {
                        new SeasonRole
                        {
                            Artist = context.Artists.SingleOrDefault(a => a.FullName == "Colin James Farrell"),
                            CharacterPlayed = "Detective Ray Velcoro"
                        }
                    },
                    TVShow = tvShow,
                };

                var season3 = new Season
                {
                    SeasonNumber = 3,
                    Episodes = 8,
                    LengthPerEpisode = 60,
                    ReleaseDate = DateTime.Parse("13 January 2019"),
                    Cast = new List<SeasonRole>()
                    {
                        new SeasonRole
                        {
                            Artist = context.Artists.SingleOrDefault(a => a.FullName == "Mahershalalhashbaz Gilmore"),
                            CharacterPlayed = "Wayne Hays"
                        }
                    },
                    TVShow = tvShow,
                };

                context.TVShows.Add(tvShow);
                context.Seasons.Add(season1);
                context.Seasons.Add(season2);
                context.Seasons.Add(season3);

                context.SaveChanges();
            }
        }
    }
}