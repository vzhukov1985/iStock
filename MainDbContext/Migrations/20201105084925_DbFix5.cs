using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCore.Migrations
{
    public partial class DbFix5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_price",
                table: "offers");

            migrationBuilder.RenameColumn(
                name: "supplier",
                table: "offers",
                newName: "Supplier");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "offers",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "offers",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "offers",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "pp2",
                table: "offers",
                newName: "Pp2");

            migrationBuilder.RenameColumn(
                name: "pp1",
                table: "offers",
                newName: "Pp1");

            migrationBuilder.RenameColumn(
                name: "nnach",
                table: "offers",
                newName: "Nnach");

            migrationBuilder.RenameColumn(
                name: "mskdnt",
                table: "offers",
                newName: "Mskdnt");

            migrationBuilder.RenameColumn(
                name: "brand",
                table: "offers",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "price_limit",
                table: "offers",
                newName: "PriceLimit");

            migrationBuilder.RenameColumn(
                name: "preorder_in_days",
                table: "offers",
                newName: "PreorderInDays");

            migrationBuilder.RenameColumn(
                name: "is_verified",
                table: "offers",
                newName: "IsVerified");

            migrationBuilder.RenameColumn(
                name: "is_favorite",
                table: "offers",
                newName: "IsFavorite");

            migrationBuilder.RenameColumn(
                name: "ware_name",
                table: "offers",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "offers",
                newName: "supplier");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "offers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "offers",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "offers",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Pp2",
                table: "offers",
                newName: "pp2");

            migrationBuilder.RenameColumn(
                name: "Pp1",
                table: "offers",
                newName: "pp1");

            migrationBuilder.RenameColumn(
                name: "Nnach",
                table: "offers",
                newName: "nnach");

            migrationBuilder.RenameColumn(
                name: "Mskdnt",
                table: "offers",
                newName: "mskdnt");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "offers",
                newName: "brand");

            migrationBuilder.RenameColumn(
                name: "PriceLimit",
                table: "offers",
                newName: "price_limit");

            migrationBuilder.RenameColumn(
                name: "PreorderInDays",
                table: "offers",
                newName: "preorder_in_days");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "offers",
                newName: "is_verified");

            migrationBuilder.RenameColumn(
                name: "IsFavorite",
                table: "offers",
                newName: "is_favorite");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "offers",
                newName: "ware_name");

            migrationBuilder.AddColumn<float>(
                name: "actual_price",
                table: "offers",
                type: "float(12,2)",
                nullable: true);
        }
    }
}
