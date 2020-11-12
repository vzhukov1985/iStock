using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DynatonePicsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Izobrazhenie",
                table: "dynatone",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Opisanie",
                table: "dynatone",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Izobrazhenie",
                table: "dynatone");

            migrationBuilder.DropColumn(
                name: "Opisanie",
                table: "dynatone");
        }
    }
}
