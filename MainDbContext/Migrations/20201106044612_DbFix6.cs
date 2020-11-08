using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ExchangeRate",
                table: "pricelists",
                type: "float(6,2)",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsExchangeRateUsed",
                table: "pricelists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "pricelists");

            migrationBuilder.DropColumn(
                name: "IsExchangeRateUsed",
                table: "pricelists");
        }
    }
}
