using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class TelegramUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "telegramusers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TelegramUserId = table.Column<long>(type: "bigint(255)", nullable: false),
                    Role = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegramusers");
        }
    }
}
