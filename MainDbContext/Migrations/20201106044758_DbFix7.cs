using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ExchangeRate",
                table: "pricelists",
                type: "float(6,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(6,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ExchangeRate",
                table: "pricelists",
                type: "float(6,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(6,2)",
                oldNullable: true);
        }
    }
}
