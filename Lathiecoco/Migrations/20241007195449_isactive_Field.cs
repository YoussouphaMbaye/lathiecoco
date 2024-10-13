using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class isactive_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Parteners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Parteners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OwnerAgents",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "OwnerAgents",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "OwnerAgents",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "OwnerAgents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CustomerWallets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "CustomerWallets",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Parteners");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Parteners");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OwnerAgents");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "OwnerAgents");

            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "OwnerAgents");

            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "OwnerAgents");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CustomerWallets");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "CustomerWallets");
        }
    }
}
