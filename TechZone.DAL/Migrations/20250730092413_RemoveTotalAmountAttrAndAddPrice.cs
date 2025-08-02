using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechZone.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalAmountAttrAndAddPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShoppingCarts",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "OrderHeaders",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
