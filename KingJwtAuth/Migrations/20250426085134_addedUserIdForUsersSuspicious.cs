using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingJwtAuth.Migrations
{
    /// <inheritdoc />
    public partial class addedUserIdForUsersSuspicious : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UsersSuspicious",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsersSuspicious");
        }
    }
}
