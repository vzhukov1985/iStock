using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "price_limit",
                table: "offers",
                type: "float(10,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(10,2)");

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "offers",
                type: "float(10,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(10,2)");

            migrationBuilder.AlterColumn<int>(
                name: "preorder_in_days",
                table: "offers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "pp2",
                table: "offers",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "pp1",
                table: "offers",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "nnach",
                table: "offers",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "mskdnt",
                table: "offers",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_favorite",
                table: "offers",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<float>(
                name: "actual_price",
                table: "offers",
                type: "float(10,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(10,2)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<float>(
                name: "Vysota",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)");

            migrationBuilder.AlterColumn<float>(
                name: "Ves",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)");

            migrationBuilder.AlterColumn<float>(
                name: "Shirina",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)");

            migrationBuilder.AlterColumn<int>(
                name: "RRC",
                table: "dynatone",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KolichestvoDlyaOpta",
                table: "dynatone",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KodGruppy",
                table: "dynatone",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Kod",
                table: "dynatone",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Dlina",
                table: "dynatone",
                type: "float(3,3)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float(3,3)");

            migrationBuilder.AlterColumn<int>(
                name: "CenaDiler",
                table: "dynatone",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Barcode",
                table: "dynatone",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "offers");

            migrationBuilder.AlterColumn<float>(
                name: "price_limit",
                table: "offers",
                type: "float(10,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "offers",
                type: "float(10,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "preorder_in_days",
                table: "offers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "pp2",
                table: "offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "pp1",
                table: "offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "nnach",
                table: "offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "mskdnt",
                table: "offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_favorite",
                table: "offers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "actual_price",
                table: "offers",
                type: "float(10,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Vysota",
                table: "dynatone",
                type: "float(3,3)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Ves",
                table: "dynatone",
                type: "float(3,3)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Shirina",
                table: "dynatone",
                type: "float(3,3)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RRC",
                table: "dynatone",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KolichestvoDlyaOpta",
                table: "dynatone",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KodGruppy",
                table: "dynatone",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Kod",
                table: "dynatone",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Dlina",
                table: "dynatone",
                type: "float(3,3)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float(3,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CenaDiler",
                table: "dynatone",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Barcode",
                table: "dynatone",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
