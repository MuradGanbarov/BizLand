using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizLand.Migrations
{
    public partial class AddLinkTableInTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceLink",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstaLink",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInLink",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TwitLink",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceLink",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "InstaLink",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LinkedInLink",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TwitLink",
                table: "Teams");
        }
    }
}
