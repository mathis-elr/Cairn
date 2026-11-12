using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cairn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SupprIdPowensInActif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Actif_IdPowens",
                table: "Actif");

            migrationBuilder.DropColumn(
                name: "IdPowens",
                table: "Actif");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPowens",
                table: "Actif",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Actif_IdPowens",
                table: "Actif",
                column: "IdPowens",
                unique: true);
        }
    }
}
