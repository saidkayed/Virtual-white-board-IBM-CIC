using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whiteboard_API.Migrations
{
    public partial class modelupdatept7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Post");
        }
    }
}
