using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class addLogCountCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "CustomerWallets",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "CustomerWallets");
        }
    }
}
