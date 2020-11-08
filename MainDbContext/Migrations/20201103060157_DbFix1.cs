using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "offers",
                newName: "status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "offers",
                newName: "Status");
        }
    }
}
