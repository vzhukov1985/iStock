using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class RemovedItemsToVerifyCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemsToVerify",
                table: "pricelists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemsToVerify",
                table: "pricelists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
