using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class AddedPleerPL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pleer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    NomerTovara = table.Column<string>(type: "varchar(255)", nullable: false),
                    Catalog = table.Column<string>(type: "varchar(255)", nullable: true),
                    KodTovara = table.Column<string>(type: "varchar(255)", nullable: true),
                    Naimenovanie = table.Column<string>(type: "varchar(255)", nullable: true),
                    Garantiya = table.Column<string>(type: "varchar(255)", nullable: true),
                    Nalichie = table.Column<int>(type: "int", nullable: false),
                    Diler1 = table.Column<int>(type: "int", nullable: false),
                    Diler2 = table.Column<int>(type: "int", nullable: false),
                    Diler3 = table.Column<int>(type: "int", nullable: false),
                    Diler4 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pleer");
        }
    }
}
