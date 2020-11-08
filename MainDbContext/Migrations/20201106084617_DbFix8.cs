using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExchangeRateUsed",
                table: "pricelists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExchangeRateUsed",
                table: "pricelists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
