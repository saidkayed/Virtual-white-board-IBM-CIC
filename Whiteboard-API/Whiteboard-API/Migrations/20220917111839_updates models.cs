using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whiteboard_API.Migrations
{
    public partial class updatesmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Post",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Post",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Post",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Post",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Post",
                newName: "Link");
        }
    }
}
