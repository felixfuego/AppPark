using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Park.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatevisitsas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsVisitaMasiva",
                table: "Visitas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdVisitaPadre",
                table: "Visitas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdVisitor",
                table: "Visitas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitaPadreId",
                table: "Visitas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitorId",
                table: "Visitas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitas_VisitaPadreId",
                table: "Visitas",
                column: "VisitaPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitas_VisitorId",
                table: "Visitas",
                column: "VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitas_Visitas_VisitaPadreId",
                table: "Visitas",
                column: "VisitaPadreId",
                principalTable: "Visitas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitas_Visitors_VisitorId",
                table: "Visitas",
                column: "VisitorId",
                principalTable: "Visitors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitas_Visitas_VisitaPadreId",
                table: "Visitas");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitas_Visitors_VisitorId",
                table: "Visitas");

            migrationBuilder.DropIndex(
                name: "IX_Visitas_VisitaPadreId",
                table: "Visitas");

            migrationBuilder.DropIndex(
                name: "IX_Visitas_VisitorId",
                table: "Visitas");

            migrationBuilder.DropColumn(
                name: "EsVisitaMasiva",
                table: "Visitas");

            migrationBuilder.DropColumn(
                name: "IdVisitaPadre",
                table: "Visitas");

            migrationBuilder.DropColumn(
                name: "IdVisitor",
                table: "Visitas");

            migrationBuilder.DropColumn(
                name: "VisitaPadreId",
                table: "Visitas");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                table: "Visitas");
        }
    }
}
