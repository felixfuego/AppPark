using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Park.Api.Migrations
{
    /// <inheritdoc />
    public partial class visitantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompaniaId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompaniaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompaniaId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdColaborador",
                table: "Users",
                column: "IdColaborador");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCompania",
                table: "Users",
                column: "IdCompania");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_Company",
                table: "Visitors",
                column: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_DocumentType_DocumentNumber",
                table: "Visitors",
                columns: new[] { "DocumentType", "DocumentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_Email",
                table: "Visitors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_IsActive",
                table: "Visitors",
                column: "IsActive");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Colaboradores_IdColaborador",
                table: "Users",
                column: "IdColaborador",
                principalTable: "Colaboradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_IdCompania",
                table: "Users",
                column: "IdCompania",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Colaboradores_IdColaborador",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_IdCompania",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdColaborador",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdCompania",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "CompaniaId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompaniaId",
                table: "Users",
                column: "CompaniaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompaniaId",
                table: "Users",
                column: "CompaniaId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
