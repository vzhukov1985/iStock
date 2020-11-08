using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pricelists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SupplierName = table.Column<string>(type: "varchar(200)", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UncheckedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PreorderInDays = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    MinStockAvail = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    IsFavorite = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Controller = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pricelists");
        }
    }
}
