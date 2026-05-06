using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class billerdateindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoice_CreatedDate",
                table: "BillerInvoices",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoice_UpdatedDate",
                table: "BillerInvoices",
                column: "UpdatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BillerInvoice_CreatedDate",
                table: "BillerInvoices");

            migrationBuilder.DropIndex(
                name: "IX_BillerInvoice_UpdatedDate",
                table: "BillerInvoices");
        }
    }
}
