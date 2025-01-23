using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class PercentagePurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PercentagePurchase",
                table: "CustomerWallets",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PercentagePurchase",
                table: "Agencies",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentagePurchase",
                table: "CustomerWallets");

            migrationBuilder.DropColumn(
                name: "PercentagePurchase",
                table: "Agencies");
        }
    }
}
