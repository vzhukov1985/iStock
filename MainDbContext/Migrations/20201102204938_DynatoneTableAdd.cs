using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DynatoneTableAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dynatone",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Kod = table.Column<int>(type: "int", nullable: false),
                    KodGruppy = table.Column<int>(type: "int", nullable: false),
                    Naimenovanie = table.Column<string>(type: "varchar(1000)", nullable: true),
                    RRC = table.Column<int>(type: "int", nullable: false),
                    CenaDiler = table.Column<int>(type: "int", nullable: false),
                    KolichestvoDlyaOpta = table.Column<int>(type: "int", nullable: false),
                    VidNomenklatury = table.Column<string>(type: "varchar(500)", nullable: true),
                    Brand = table.Column<string>(type: "varchar(500)", nullable: true),
                    ModelSModifikaciyey = table.Column<string>(type: "varchar(500)", nullable: true),
                    Articul = table.Column<string>(type: "varchar(150)", nullable: true),
                    Barcode = table.Column<long>(type: "bigint", nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Modifikaciya = table.Column<string>(type: "varchar(500)", nullable: true),
                    KodSPrefixom = table.Column<string>(type: "varchar(150)", nullable: true),
                    StranaProishojdenia = table.Column<string>(type: "varchar(150)", nullable: true),
                    Ves = table.Column<float>(type: "float(3,3)", nullable: false),
                    Dlina = table.Column<float>(type: "float(3,3)", nullable: false),
                    Shirina = table.Column<float>(type: "float(3,3)", nullable: false),
                    Vysota = table.Column<float>(type: "float(3,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dynatone");
        }
    }
}
