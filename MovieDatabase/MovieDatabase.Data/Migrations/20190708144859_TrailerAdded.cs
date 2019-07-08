using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDatabase.Data.Migrations
{
    public partial class TrailerAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrailerLink",
                table: "TVShows",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerLink",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrailerLink",
                table: "TVShows");

            migrationBuilder.DropColumn(
                name: "TrailerLink",
                table: "Movies");
        }
    }
}
