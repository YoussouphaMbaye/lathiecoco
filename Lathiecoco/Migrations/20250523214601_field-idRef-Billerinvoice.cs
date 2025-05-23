using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class fieldidRefBillerinvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdReference",
                table: "BillerInvoices",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdReference",
                table: "BillerInvoices");
        }
    }
}
