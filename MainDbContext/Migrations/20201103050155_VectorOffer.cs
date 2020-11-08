using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class VectorOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sku = table.Column<string>(type: "varchar(150)", nullable: true),
                    brand = table.Column<string>(type: "varchar(300)", nullable: true),
                    ware_name = table.Column<string>(type: "varchar(1000)", nullable: true),
                    price = table.Column<float>(type: "float(10,2)", nullable: false),
                    price_limit = table.Column<float>(type: "float(10,2)", nullable: false),
                    actual_price = table.Column<float>(type: "float(10,2)", nullable: false),
                    pp1 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    pp2 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    mskdnt = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    nnach = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    preorder_in_days = table.Column<int>(type: "int", nullable: false),
                    is_favorite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    supplier = table.Column<string>(type: "varchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offers");
        }
    }
}
