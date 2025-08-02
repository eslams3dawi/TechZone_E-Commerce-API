using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechZone.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditPriceAttributeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceForMoreThan5",
                table: "Products",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "PriceForMoreThan5");
        }
    }
}
