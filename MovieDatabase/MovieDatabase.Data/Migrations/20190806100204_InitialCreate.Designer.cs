﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieDatabase.Data;

namespace MovieDatabase.Data.Migrations
{
    [DbContext(typeof(MovieDatabaseDbContext))]
    [Migration("20190806100204_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MovieDatabase.Domain.Announcement", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("Date");

                    b.Property<string>("ImageLink");

                    b.Property<string>("OfficialArticleLink")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("MovieDatabase.Domain.Artist", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(10000);

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhotoLink")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("MovieDatabase.Domain.Genre", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MovieDatabase.Domain.Movie", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoverImageLink")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("DirectorId")
                        .IsRequired();

                    b.Property<string>("GenreId")
                        .IsRequired();

                    b.Property<int>("Length");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("TrailerLink")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DirectorId");

                    b.HasIndex("GenreId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieDatabaseUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AvatarLink")
                        .IsRequired();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieReview", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("UserId");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(10000);

                    b.Property<DateTime>("Date");

                    b.Property<int>("Rating");

                    b.HasKey("MovieId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("MovieReviews");
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieRole", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("ArtistId");

                    b.Property<string>("CharacterPlayed")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("MovieId", "ArtistId");

                    b.HasIndex("ArtistId");

                    b.ToTable("MovieRoles");
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieUser", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("UserId");

                    b.HasKey("MovieId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("MovieUsers");
                });

            modelBuilder.Entity("MovieDatabase.Domain.Season", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Episodes");

                    b.Property<int>("LengthPerEpisode");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("SeasonNumber");

                    b.Property<string>("TVShowId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("TVShowId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("MovieDatabase.Domain.SeasonReview", b =>
                {
                    b.Property<string>("SeasonId");

                    b.Property<string>("UserId");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(10000);

                    b.Property<DateTime>("Date");

                    b.Property<int>("Rating");

                    b.HasKey("SeasonId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("SeasonReviews");
                });

            modelBuilder.Entity("MovieDatabase.Domain.SeasonRole", b =>
                {
                    b.Property<string>("SeasonId");

                    b.Property<string>("ArtistId");

                    b.Property<string>("CharacterPlayed")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("SeasonId", "ArtistId");

                    b.HasIndex("ArtistId");

                    b.ToTable("SeasonRoles");
                });

            modelBuilder.Entity("MovieDatabase.Domain.TVShow", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoverImageLink")
                        .IsRequired();

                    b.Property<string>("CreatorId")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("GenreId")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TrailerLink")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("GenreId");

                    b.ToTable("TVShows");
                });

            modelBuilder.Entity("MovieDatabase.Domain.TVShowUser", b =>
                {
                    b.Property<string>("TVShowId");

                    b.Property<string>("UserId");

                    b.HasKey("TVShowId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TVShowUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieDatabase.Domain.Movie", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Artist", "Director")
                        .WithMany("MoviesDirected")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieDatabase.Domain.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieReview", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Movie", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser", "User")
                        .WithMany("MovieReviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieRole", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Artist", "Artist")
                        .WithMany("MovieRoles")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MovieDatabase.Domain.Movie", "Movie")
                        .WithMany("Cast")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MovieDatabase.Domain.MovieUser", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser", "User")
                        .WithMany("WatchlistedMovies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MovieDatabase.Domain.Season", b =>
                {
                    b.HasOne("MovieDatabase.Domain.TVShow", "TVShow")
                        .WithMany("Seasons")
                        .HasForeignKey("TVShowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieDatabase.Domain.SeasonReview", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Season", "Season")
                        .WithMany("Reviews")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser", "User")
                        .WithMany("SeasonReviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MovieDatabase.Domain.SeasonRole", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Artist", "Artist")
                        .WithMany("SeasonRoles")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MovieDatabase.Domain.Season", "Season")
                        .WithMany("Cast")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MovieDatabase.Domain.TVShow", b =>
                {
                    b.HasOne("MovieDatabase.Domain.Artist", "Creator")
                        .WithMany("TVShowsCreated")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieDatabase.Domain.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieDatabase.Domain.TVShowUser", b =>
                {
                    b.HasOne("MovieDatabase.Domain.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MovieDatabase.Domain.MovieDatabaseUser", "User")
                        .WithMany("WatchlistedTVShows")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
