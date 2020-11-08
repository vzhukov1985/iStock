using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "price_limit",
                table: "offers",
                type: "float(12,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "offers",
                type: "float(12,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "actual_price",
                table: "offers",
                type: "float(12,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Vysota",
                table: "dynatone",
                type: "float(6,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Ves",
                table: "dynatone",
                type: "float(6,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Shirina",
                table: "dynatone",
                type: "float(6,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Dlina",
                table: "dynatone",
                type: "float(6,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "price_limit",
                table: "offers",
                type: "float(10,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "offers",
                type: "float(10,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "actual_price",
                table: "offers",
                type: "float(10,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Vysota",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Ves",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Shirina",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(6,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Dlina",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(6,3)",
                oldNullable: true);
        }
    }
}
