using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingJwtAuth.Migrations
{
    /// <inheritdoc />
    public partial class makeConnectedBetweenUsersUserRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_UserId",
                table: "UserRefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshToken_Users_UserId",
                table: "UserRefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_UserRefreshToken_UserId",
                table: "UserRefreshToken");
        }
    }
}
