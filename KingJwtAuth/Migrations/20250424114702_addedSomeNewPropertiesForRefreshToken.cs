using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingJwtAuth.Migrations
{
    /// <inheritdoc />
    public partial class addedSomeNewPropertiesForRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IP",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "RefreshToken");
        }
    }
}
