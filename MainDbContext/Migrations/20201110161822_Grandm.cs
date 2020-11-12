using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class Grandm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grandm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Brand = table.Column<string>(type: "varchar(255)", nullable: true),
                    CategoryName = table.Column<string>(type: "varchar(255)", nullable: true),
                    CenaDiler = table.Column<int>(type: "int", nullable: true),
                    Kornevaya = table.Column<string>(type: "varchar(255)", nullable: true),
                    Podkategoriya1 = table.Column<string>(type: "varchar(255)", nullable: true),
                    Podkategoriya2 = table.Column<string>(type: "varchar(255)", nullable: true),
                    Podkategoriya3 = table.Column<string>(type: "varchar(255)", nullable: true),
                    Podkategoriya4 = table.Column<string>(type: "varchar(255)", nullable: true),
                    IdTovara = table.Column<int>(type: "int", nullable: true),
                    NazvanieTovara = table.Column<string>(type: "varchar(500)", nullable: true),
                    URL = table.Column<string>(type: "varchar(500)", nullable: true),
                    KratkoeOpisanie = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Izobrazheniya = table.Column<string>(type: "varchar(500)", nullable: true),
                    SvoistvoRazmer = table.Column<string>(type: "varchar(255)", nullable: true),
                    SvoistvoCvet = table.Column<string>(type: "varchar(255)", nullable: true),
                    Articul = table.Column<string>(type: "varchar(255)", nullable: true),
                    CenaProdazhi = table.Column<string>(type: "varchar(255)", nullable: true),
                    Ostatok = table.Column<int>(type: "int", nullable: true),
                    Ves = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grandm");
        }
    }
}
