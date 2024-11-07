using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M6_MovieClub.Data.Migrations
{
    public partial class userMapToIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_OwnerId",
                table: "Movies",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_OwnerId",
                table: "Movies",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_OwnerId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_OwnerId",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
