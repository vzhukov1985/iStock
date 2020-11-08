using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "pricelists",
                newName: "LastPull");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastPull",
                table: "pricelists",
                newName: "LastUpdate");
        }
    }
}
