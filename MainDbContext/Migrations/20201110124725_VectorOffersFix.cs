using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class VectorOffersFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PricelistId",
                table: "offers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricelistId",
                table: "offers");
        }
    }
}
