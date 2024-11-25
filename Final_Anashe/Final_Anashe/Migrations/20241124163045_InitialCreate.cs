using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Anashe.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuraciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuraciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "provincias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sucursales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCiudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdTipo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProvincia = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreTitular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoTitular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sucursales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sucursales_provincias_IdProvincia",
                        column: x => x.IdProvincia,
                        principalTable: "provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sucursales_tipos_IdTipo",
                        column: x => x.IdTipo,
                        principalTable: "tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sucursales_IdProvincia",
                table: "sucursales",
                column: "IdProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_sucursales_IdTipo",
                table: "sucursales",
                column: "IdTipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuraciones");

            migrationBuilder.DropTable(
                name: "sucursales");

            migrationBuilder.DropTable(
                name: "provincias");

            migrationBuilder.DropTable(
                name: "tipos");
        }
    }
}
