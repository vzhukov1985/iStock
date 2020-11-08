using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UncheckedCount",
                table: "pricelists",
                newName: "ItemsToVerify");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_verified",
                table: "offers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_verified",
                table: "offers");

            migrationBuilder.RenameColumn(
                name: "ItemsToVerify",
                table: "pricelists",
                newName: "UncheckedCount");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "offers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }
    }
}
