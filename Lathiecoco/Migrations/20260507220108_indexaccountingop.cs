using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class indexaccountingop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgent_CreatedDate",
                table: "InvoiceWalletAgents",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgent_UpdatedDate",
                table: "InvoiceWalletAgents",
                column: "UpdatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMaster_CreatedDate",
                table: "InvoiceStartupMasters",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMaster_UpdatedDate",
                table: "InvoiceStartupMasters",
                column: "UpdatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallet_CreatedDate",
                table: "AccountingOpWallets",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallet_UpdatedDate",
                table: "AccountingOpWallets",
                column: "UpdatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InvoiceWalletAgent_CreatedDate",
                table: "InvoiceWalletAgents");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceWalletAgent_UpdatedDate",
                table: "InvoiceWalletAgents");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceStartupMaster_CreatedDate",
                table: "InvoiceStartupMasters");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceStartupMaster_UpdatedDate",
                table: "InvoiceStartupMasters");

            migrationBuilder.DropIndex(
                name: "IX_AccountingOpWallet_CreatedDate",
                table: "AccountingOpWallets");

            migrationBuilder.DropIndex(
                name: "IX_AccountingOpWallet_UpdatedDate",
                table: "AccountingOpWallets");
        }
    }
}
