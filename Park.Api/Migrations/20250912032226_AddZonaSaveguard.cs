using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Park.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddZonaSaveguard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdZonaAsignada",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZonaAsignadaId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ZonaAsignadaId",
                table: "Users",
                column: "ZonaAsignadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Zonas_ZonaAsignadaId",
                table: "Users",
                column: "ZonaAsignadaId",
                principalTable: "Zonas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Zonas_ZonaAsignadaId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ZonaAsignadaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdZonaAsignada",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ZonaAsignadaId",
                table: "Users");
        }
    }
}
