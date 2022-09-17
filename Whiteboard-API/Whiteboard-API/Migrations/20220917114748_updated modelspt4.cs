using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whiteboard_API.Migrations
{
    public partial class updatedmodelspt4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isAnonymous",
                table: "Post",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "isAnonymous",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
