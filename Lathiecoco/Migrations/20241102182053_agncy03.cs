using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class agncy03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FkIdAgencyUser",
                table: "InvoiceStartupMasters",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaster",
                table: "InvoiceStartupMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FkIdAgency",
                table: "CustomerWallets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMasters_FkIdAgencyUser",
                table: "InvoiceStartupMasters",
                column: "FkIdAgencyUser");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_FkIdAgency",
                table: "CustomerWallets",
                column: "FkIdAgency");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWallets_Agencies_FkIdAgency",
                table: "CustomerWallets",
                column: "FkIdAgency",
                principalTable: "Agencies",
                principalColumn: "IdAgency",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceStartupMasters_AgencyUsers_FkIdAgencyUser",
                table: "InvoiceStartupMasters",
                column: "FkIdAgencyUser",
                principalTable: "AgencyUsers",
                principalColumn: "IdAgencyUser",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWallets_Agencies_FkIdAgency",
                table: "CustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceStartupMasters_AgencyUsers_FkIdAgencyUser",
                table: "InvoiceStartupMasters");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceStartupMasters_FkIdAgencyUser",
                table: "InvoiceStartupMasters");

            migrationBuilder.DropIndex(
                name: "IX_CustomerWallets_FkIdAgency",
                table: "CustomerWallets");

            migrationBuilder.DropColumn(
                name: "FkIdAgencyUser",
                table: "InvoiceStartupMasters");

            migrationBuilder.DropColumn(
                name: "IsMaster",
                table: "InvoiceStartupMasters");

            migrationBuilder.DropColumn(
                name: "FkIdAgency",
                table: "CustomerWallets");
        }
    }
}
