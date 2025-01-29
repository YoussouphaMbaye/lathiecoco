using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class tokenRefresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDateTokenRefresh",
                table: "OwnerAgents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenRefresh",
                table: "OwnerAgents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDateTokenRefresh",
                table: "AgencyUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenRefresh",
                table: "AgencyUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDateTokenRefresh",
                table: "OwnerAgents");

            migrationBuilder.DropColumn(
                name: "TokenRefresh",
                table: "OwnerAgents");

            migrationBuilder.DropColumn(
                name: "ExpireDateTokenRefresh",
                table: "AgencyUsers");

            migrationBuilder.DropColumn(
                name: "TokenRefresh",
                table: "AgencyUsers");
        }
    }
}
