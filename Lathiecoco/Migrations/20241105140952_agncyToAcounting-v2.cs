using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class agncyToAcountingv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceStartupMasters_CustomerWallets_FkIdAgent",
                table: "InvoiceStartupMasters");

            migrationBuilder.AlterColumn<string>(
                name: "FkIdAgent",
                table: "InvoiceStartupMasters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceStartupMasters_CustomerWallets_FkIdAgent",
                table: "InvoiceStartupMasters",
                column: "FkIdAgent",
                principalTable: "CustomerWallets",
                principalColumn: "IdCustomerWallet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceStartupMasters_CustomerWallets_FkIdAgent",
                table: "InvoiceStartupMasters");

            migrationBuilder.AlterColumn<string>(
                name: "FkIdAgent",
                table: "InvoiceStartupMasters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceStartupMasters_CustomerWallets_FkIdAgent",
                table: "InvoiceStartupMasters",
                column: "FkIdAgent",
                principalTable: "CustomerWallets",
                principalColumn: "IdCustomerWallet",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
