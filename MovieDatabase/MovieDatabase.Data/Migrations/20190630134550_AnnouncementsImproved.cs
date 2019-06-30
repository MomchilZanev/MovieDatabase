using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDatabase.Data.Migrations
{
    public partial class AnnouncementsImproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Seasons",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Announcements",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficialArticleLink",
                table: "Announcements",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "OfficialArticleLink",
                table: "Announcements");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Seasons",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
