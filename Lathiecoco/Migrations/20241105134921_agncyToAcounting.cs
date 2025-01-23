using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class agncyToAcounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FkIdAccounting",
                table: "Agencies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_FkIdAccounting",
                table: "Agencies",
                column: "FkIdAccounting",
                unique: true,
                filter: "[FkIdAccounting] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Agencies_Accountings_FkIdAccounting",
                table: "Agencies",
                column: "FkIdAccounting",
                principalTable: "Accountings",
                principalColumn: "IdAccounting");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencies_Accountings_FkIdAccounting",
                table: "Agencies");

            migrationBuilder.DropIndex(
                name: "IX_Agencies_FkIdAccounting",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "FkIdAccounting",
                table: "Agencies");
        }
    }
}
