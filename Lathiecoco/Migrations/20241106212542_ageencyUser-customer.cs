using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class ageencyUsercustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FkIdAgencyUser",
                table: "CustomerWallets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_FkIdAgencyUser",
                table: "CustomerWallets",
                column: "FkIdAgencyUser");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWallets_AgencyUsers_FkIdAgencyUser",
                table: "CustomerWallets",
                column: "FkIdAgencyUser",
                principalTable: "AgencyUsers",
                principalColumn: "IdAgencyUser",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWallets_AgencyUsers_FkIdAgencyUser",
                table: "CustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_CustomerWallets_FkIdAgencyUser",
                table: "CustomerWallets");

            migrationBuilder.DropColumn(
                name: "FkIdAgencyUser",
                table: "CustomerWallets");
        }
    }
}
