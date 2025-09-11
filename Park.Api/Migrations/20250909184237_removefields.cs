using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Park.Api.Migrations
{
    /// <inheritdoc />
    public partial class removefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCentros_Centros_IdCentro",
                table: "CompanyCentros");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCentros_Companies_IdCompania",
                table: "CompanyCentros");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCentros_IdCentro",
                table: "CompanyCentros");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCentros_IdCompania_IdCentro",
                table: "CompanyCentros");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Email",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "CentroId",
                table: "CompanyCentros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompaniaId",
                table: "CompanyCentros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCentros_CentroId",
                table: "CompanyCentros",
                column: "CentroId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCentros_CompaniaId",
                table: "CompanyCentros",
                column: "CompaniaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCentros_Centros_CentroId",
                table: "CompanyCentros",
                column: "CentroId",
                principalTable: "Centros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCentros_Companies_CompaniaId",
                table: "CompanyCentros",
                column: "CompaniaId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCentros_Centros_CentroId",
                table: "CompanyCentros");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCentros_Companies_CompaniaId",
                table: "CompanyCentros");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCentros_CentroId",
                table: "CompanyCentros");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCentros_CompaniaId",
                table: "CompanyCentros");

            migrationBuilder.DropColumn(
                name: "CentroId",
                table: "CompanyCentros");

            migrationBuilder.DropColumn(
                name: "CompaniaId",
                table: "CompanyCentros");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Companies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Companies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCentros_IdCentro",
                table: "CompanyCentros",
                column: "IdCentro");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCentros_IdCompania_IdCentro",
                table: "CompanyCentros",
                columns: new[] { "IdCompania", "IdCentro" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Email",
                table: "Companies",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCentros_Centros_IdCentro",
                table: "CompanyCentros",
                column: "IdCentro",
                principalTable: "Centros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCentros_Companies_IdCompania",
                table: "CompanyCentros",
                column: "IdCompania",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
